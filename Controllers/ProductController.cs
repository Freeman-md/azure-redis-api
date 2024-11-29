using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Contracts.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using StackExchange.Redis;

namespace azure_redis_api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly IRedisCacheService _redisCacheService;

        public ProductController(ProductContext context, IRedisCacheService redisCacheService)
        {
            _context = context;
            _redisCacheService = redisCacheService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var cacheKey = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var cachedProducts = await _redisCacheService.Get(cacheKey);
            if (!string.IsNullOrEmpty(cachedProducts))
            {
                var productsFromCache = JsonSerializer.Deserialize<IEnumerable<Product>>(cachedProducts);
                return Ok(productsFromCache);
            }


            var products = await _context.Products.ToListAsync();

            await _redisCacheService.Set(cacheKey, JsonSerializer.Serialize<IEnumerable<Product>>(products), TimeSpan.FromSeconds(60));

            return products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var cacheKey = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var cachedProduct = await _redisCacheService.Get(cacheKey);
            if (!string.IsNullOrEmpty(cachedProduct))
            {
                var productFromCache = JsonSerializer.Deserialize<Product>(cachedProduct);
                return Ok(productFromCache);
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var serializedProduct = JsonSerializer.Serialize(product);
            await _redisCacheService.Set(cacheKey, serializedProduct, TimeSpan.FromSeconds(10));

            return product;
        }


        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}

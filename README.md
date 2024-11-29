# Azure Redis API

A simple .NET API demonstrating the integration of Azure Cache for Redis to enhance performance by caching frequently accessed data. This project serves as a practical example of how Redis caching can improve response times and reduce database load.

## Features

- Fetch products from an in-memory SQLite database.
- Redis caching to store product data for faster responses.
- Automatic caching with configurable expiration times.
- Simplified code structure with reusable services for caching.
- Swagger for API documentation and testing.

## Endpoints

### Get All Products
- **GET** `/api/products`
- Fetches all products. Cached results are served if available, reducing database hits.

### Get Product by ID
- **GET** `/api/products/{id}`
- Fetches a single product by its ID. If the product is cached, it is served directly from Redis.

### Create a Product
- **POST** `/api/products`
- Adds a new product to the database. (No caching is applied here.)

### Delete a Product
- **DELETE** `/api/products/{id}`
- Deletes a product by its ID from the database. (Cache invalidation is not implemented for simplicity.)

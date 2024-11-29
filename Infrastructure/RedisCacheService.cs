using Contracts.Infrastructure;
using StackExchange.Redis;

namespace Infrastructure;

public class RedisCacheService : IRedisCacheService {
    private readonly IDatabase _redisDatabase;
    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer) {
        _redisDatabase = connectionMultiplexer.GetDatabase();
    }

    public async Task<bool> Set(string key, RedisValue value, TimeSpan? ttl = null) {
        return await _redisDatabase.StringSetAsync(key, value, ttl);
    }

    public async Task<RedisValue?> Get(string key) {
        return await _redisDatabase.StringGetAsync(key);
    }

    public async Task<bool> DeleteAsync(string key) {
        return await _redisDatabase.KeyDeleteAsync(key);
    }

    public async Task<bool> KeyExistsAsync(string key) {
        return await _redisDatabase.KeyExistsAsync(key);
    }
}
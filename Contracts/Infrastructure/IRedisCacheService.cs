using StackExchange.Redis;

namespace Contracts.Infrastructure;

public interface IRedisCacheService {
    Task<bool> Set(string key, RedisValue value, TimeSpan? expiry = null);
    Task<RedisValue?> Get(string key);
    Task<bool> DeleteAsync(string key);
    Task<bool> KeyExistsAsync(string key);
}
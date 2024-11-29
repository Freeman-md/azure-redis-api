using Microsoft.Extensions.DependencyInjection;
using Contracts.Infrastructure;
using Infrastructure;
using StackExchange.Redis;

namespace Extensions;

public static class RedisServiceExtensions
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString)) {
            throw new ArgumentNullException(nameof(connectionString));
        } 
        
        var multiplexer = ConnectionMultiplexer.Connect(connectionString);
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        services.AddSingleton<IRedisCacheService, RedisCacheService>();

        return services;
    }
}
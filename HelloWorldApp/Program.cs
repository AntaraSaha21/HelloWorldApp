using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Distributed;

// 1. Build a simple host container to handle Dependency Injection
var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    // 2. Register the Redis cache service
    services.AddStackExchangeRedisCache(options =>
    {
        // Change "localhost:6379" to your Redis instance address if it is remote/Docker
        options.Configuration = "localhost:6379"; 
        options.InstanceName = "ConsoleApp_";
    });
});

var host = builder.Build();

// 3. Resolve the distributed cache service
var cache = host.Services.GetRequiredService<IDistributedCache>();

// --- Example Usage ---
string cacheKey = "hello_key";
string valueToCache = "Hello from Redis caching in .NET!";

Console.WriteLine("Saving data to Redis cache...");
var options = new DistributedCacheEntryOptions()
    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

// Save data
await cache.SetStringAsync(cacheKey, valueToCache, options);

Console.WriteLine("Fetching data back from Redis cache...");
// Retrieve data
string? cachedValue = await cache.GetStringAsync(cacheKey);

Console.WriteLine($"Retrieved Value: {cachedValue}");

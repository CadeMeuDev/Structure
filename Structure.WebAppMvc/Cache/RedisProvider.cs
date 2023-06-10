using StackExchange.Redis;

namespace Structure.WebAppMvc.Cache
{
    public class RedisProvider
    {
        public IDatabase Cache { get; }

        public RedisProvider(IConfiguration configuration)
        {
            var coonnectionString = configuration.GetConnectionString("Redis")
                 ?? throw new InvalidOperationException("Connection string 'redis' not found.");

            var redis = ConnectionMultiplexer.Connect(coonnectionString);
            Cache = redis.GetDatabase();
        }
    }
}
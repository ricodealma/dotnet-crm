using Dotnet.Crm.Domain.SeedWork;
using Dotnet.Crm.Domain.SeedWork.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Dotnet.Crm.Infra.External
{
    public class DistributedMemoryCacheDAO(IConnectionMultiplexer connectionMultiplexer, ILogger<DistributedMemoryCacheDAO> logger) : IDistributedMemoryCacheDAO
    {
        private readonly IDatabase? _redisDatabase = connectionMultiplexer?.GetDatabase();
        private readonly ILogger<DistributedMemoryCacheDAO> _logger = logger;

        public void SetValue(string key, string value, TimeSpan expiration)
        {
            try
            {
                if (_redisDatabase is null)
                {
                    _logger.LogError("Redis is unavailable. As a result, set value operation will be skipped.");
                    return;
                }
                _redisDatabase!.StringSet($"{Constant.APP_REDIS_CACHE_ENTITY_BASE_NAME}{key}", value, expiration, When.Always, CommandFlags.FireAndForget);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, JsonConvert.SerializeObject(e));
            }


        }
        public bool TryGetValue<T>(string key, out T? value)
        {
            value = default;
            try
            {
                if (_redisDatabase is null)
                {
                    _logger.LogError("Redis is unavailable. As a result, try get value operation will be skipped.");
                    return false;
                }
                string? returnedValue = _redisDatabase!.StringGet($"{Constant.APP_REDIS_CACHE_ENTITY_BASE_NAME}{key}");

                if (string.IsNullOrEmpty(returnedValue))
                    return false;

                if (typeof(T) == typeof(string))
                {
                    value = (T)(object)returnedValue;
                    return true;
                }

                value = JsonHelper.Deserialize<T>(returnedValue);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, JsonConvert.SerializeObject(e));
                return false;
            }
        }
    }
}

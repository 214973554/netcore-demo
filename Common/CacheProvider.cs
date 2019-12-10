using log4net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;
using System;

namespace Common
{
    public interface ICacheProvider
    {
        void Set(string key, object value);
        object Get(string key);
        bool Remove(string key);

    }

    public interface IRedisProvider
    {
        bool Add<T>(string key, T value);
        T Select<T>(string key);
        bool Delete(string key);

        bool RedisEnable { get; }
    }

    public class HttpCacheProvider : ICacheProvider
    {
        private IMemoryCache memoryCache;
        private IConfiguration configuration;
        public HttpCacheProvider(IMemoryCache memoryCache, IConfiguration configuration)
        {
            this.memoryCache = memoryCache;
            this.configuration = configuration;
        }
        public void Set(string key, object value)
        {
            if (memoryCache.Get(key) == null)
            {
                int days = Convert.ToInt32(configuration["Vcredit:Cache:MemoryCache:SlidingExpirationDays"]);
                memoryCache.Set(key, value, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.MaxValue,
                    SlidingExpiration = TimeSpan.FromDays(days)
                });
            }
        }

        public object Get(string key)
        {
            return memoryCache.Get(key);
        }

        public bool Remove(string key)
        {
            try
            {
                memoryCache.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }

    public class RedisCacheProvider : IRedisProvider
    {
        private PooledRedisClientManager redisManager;
        private bool redisEnable;
        private IConfiguration configuration;
        private Log4netHelper log;
        private int redisExpries;
        public RedisCacheProvider(IConfiguration configuration, ILog ilog)
        {
            this.configuration = configuration;
            this.log = new Log4netHelper(ilog, this.GetType());

            
            if (!int.TryParse(configuration["Vcredit:Cache:Redis:ExpriesDays"], out redisExpries))
            {
                redisExpries = 3;
            }

            string enableConfig = configuration["Vcredit:Cache:Redis:Enable"];
            bool enable = !string.IsNullOrEmpty(enableConfig) && enableConfig.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            if (enable)
            {
                Close();
                try
                {
                    string[] RedisReadWriteHosts = new string[] { configuration["Vcredit:Cache:Redis:ReadWriteHosts"] };
                    string[] RedisReadOnlyHosts = new string[] { configuration["Vcredit:Cache:Redis:ReadOnlyHosts"] };
                    RedisClientManagerConfig config = new RedisClientManagerConfig()
                    {
                        AutoStart = false,
                        MaxReadPoolSize = 200,
                        MaxWritePoolSize = 50
                    };
                    redisManager = new PooledRedisClientManager(RedisReadWriteHosts, RedisReadOnlyHosts, config); //readWriteHosts
                    redisManager.Start();
                }
                catch (Exception ex)
                {
                    log.Log("实例化PooledRedisClientManager异常。", LogType.Error, ex);
                }
            }
            else
            {
                Close();
            }

            redisEnable = enable;
        }
        public void Close()
        {
            try
            {
                if (redisManager != null)
                {
                    redisManager.Dispose();
                }
            }
            catch { }
        }
        public bool RedisEnable
        {
            get
            {
                return redisEnable;
            }
        }

        public bool Add<T>(string key, T value)
        {
            if (RedisEnable)
            {
                return redisManager.GetClient().Set<string>(key, SerializationHelper.SerializeToXml(value), DateTime.Now.AddDays(redisExpries));
            }
            else
            {
                return false;
            }
        }

        public T Select<T>(string key)
        {
            if (RedisEnable)
            {
                return SerializationHelper.DeserializeFromXml<T>(redisManager.GetReadOnlyClient().Get<string>(key));
            }
            else
            {
                return default(T);
            }
        }

        public bool Delete(string key)
        {

            return RedisEnable ? redisManager.GetClient().Remove(key) : false;
        }
    }

    public class CacheProvider : ICacheProvider, IRedisProvider
    {
        private HttpCacheProvider httpCache;
        private RedisCacheProvider redisCache;
        private static CacheProvider cache;

        public HttpCacheProvider HttpCache
        {
            get
            {
                return httpCache;
            }
        }

        public RedisCacheProvider RedisCache
        {
            get
            {
                return redisCache;
            }
        }

        public bool RedisEnable => redisCache.RedisEnable;

        private CacheProvider(IMemoryCache memoryCache,IConfiguration configuration,ILog ilog)
        {
            this.httpCache = new HttpCacheProvider(memoryCache,configuration);
            this.redisCache = new RedisCacheProvider(configuration,ilog);
        }

        private static object _lock = new object();

        public static CacheProvider GetInstance(IMemoryCache memoryCache, IConfiguration configuration, ILog ilog)
        {
            if (cache == null)
            {
                lock (_lock)
                {
                    if (cache == null)
                    {
                        cache = new CacheProvider(memoryCache,configuration,ilog);
                    }
                }
            }

            return cache;
        }


        public void Set(string key, object value)
        {
            httpCache.Set(key, value);
        }

        public object Get(string key)
        {
            return httpCache.Get(key);
        }

        public bool Remove(string key)
        {
            return httpCache.Remove(key);
        }

        public bool Add<T>(string key, T value)
        {
            return redisCache.Add<T>(key, value);
        }
        public T Select<T>(string key)
        {
            return redisCache.Select<T>(key);
        }
        public bool Delete(string key)
        {
            return redisCache.Delete(key);
        }

    }

}
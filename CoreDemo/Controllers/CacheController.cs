using Common;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CoreDemo.Controllers
{
    /// <summary>
    /// 缓存示例
    /// </summary>
    public class CacheController : ApiControllerBase
    {
        private CacheProvider cacheProvider;
        private HttpCacheProvider httpCacheProvider;
        private RedisCacheProvider redisCacheProvider;

        public CacheController(ILog ilog, IMemoryCache memoryCache,IConfiguration configuration) : base(ilog)
        {
            this.cacheProvider = CacheProvider.GetInstance(memoryCache,configuration,ilog);
            this.httpCacheProvider = cacheProvider.HttpCache;
            this.redisCacheProvider = cacheProvider.RedisCache;
        }

        [HttpGet,Route("TestHttpCache")]
        public ActionResult TestHttpCache()
        {
            httpCacheProvider.Set("key1", "hello world");
            httpCacheProvider.Set("date.now", DateTime.Now);

            Dictionary<string, object> cacheContext = new Dictionary<string, object>();
            string[] keys = new string[] { "key1", "date.now" };
            foreach(string key in keys)
            {
                cacheContext.Add(key, httpCacheProvider.Get(key));
            }

            return new JsonResult(cacheContext);
        }

        [HttpGet,Route("GetHttpCache")]
        public ActionResult GetHttpCache()
        {
            Dictionary<string, object> cacheContext = new Dictionary<string, object>();
            string[] keys = new string[] { "key1", "date.now" };
            foreach (string key in keys)
            {
                cacheContext.Add(key, httpCacheProvider.Get(key));
            }

            return new JsonResult(cacheContext);
        }

        [HttpGet, Route("TestRedisCache")]
        public ActionResult TestRedisCache()
        {
            redisCacheProvider.Add<string>("vcredit:rc:dotnet.key1", "hello world");
            redisCacheProvider.Add<string>("vcredit:rc:dotnet.date.now", DateTime.Now.ToLongDateString());

            Dictionary<string, object> cacheContext = new Dictionary<string, object>();
            string[] keys = new string[] { "vcredit:rc:dotnet.key1", "vcredit:rc:dotnet.date.now" };
            foreach (string key in keys)
            {
                cacheContext.Add(key, redisCacheProvider.Select<string>(key));
            }

            return new JsonResult(cacheContext);
        }

        [HttpGet, Route("GetRedisCache")]
        public ActionResult GetRedisCache()
        {
            Dictionary<string, object> cacheContext = new Dictionary<string, object>();
            string[] keys = new string[] { "vcredit:rc:dotnet.key1", "vcredit:rc:dotnet.date.now" };
            foreach (string key in keys)
            {
                cacheContext.Add(key, redisCacheProvider.Select<string>(key));
            }

            return new JsonResult(cacheContext);
        }
    }
}
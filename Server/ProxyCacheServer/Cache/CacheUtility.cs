using System;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceModel;

namespace ProxyCacheServer.Cache
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    internal static class CacheUtility
    {
        private static readonly ObjectCache CacheData = MemoryCache.Default;

        public static T GetFromCache<T>(string key) where T : class
        {
            if (!CacheData.Contains(key)) return null;

            Console.WriteLine($"Data retrieved from cache with key: {key}.");
            return CacheData.Get(key) as T;
        }

        public static void SetToCache<T>(string newKey, T data, int durationInMinutes) where T : class
        {
            if(string.IsNullOrEmpty(newKey)) return;
            if(CacheData.Contains(newKey)) CacheData.Remove(newKey);
            CacheData.Set(newKey, data, DateTimeOffset.Now.AddMinutes(durationInMinutes));
            Console.WriteLine($"Data retrieved from API. Data cached with key: {newKey}.");
        }

        public static void ClearCache() => CacheData.ToList().ForEach(element => CacheData.ToList().Remove(element));
    }
}

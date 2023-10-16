using SomeBlog.Data;
using SomeBlog.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;

namespace SomeBlog.AdGuard
{
    public class CacheHelper
    {
        ICache cache;
        AdBannedIpData adBannedIpData;
        object lockObject;

        public CacheHelper(ICache cache, AdBannedIpData adBannedIpData)
        {
            this.cache = cache;
            this.adBannedIpData = adBannedIpData;
            this.lockObject = new object();
        }

        private string adBannedIps_cache = "adBannedIps_cache";
        public bool AdBannedIpsClear() { return Clear(adBannedIps_cache); }
        public List<Model.AdBannedIp> AdBannedIps
        {
            get
            {
                var fromCache = Get<List<Model.AdBannedIp>>(adBannedIps_cache);

                if (fromCache == null)
                {
                    lock (lockObject)
                    {
                        if (fromCache == null)
                        {
                            fromCache = adBannedIpData.GetAll();

                            if (fromCache != null)
                                Set(adBannedIps_cache, fromCache, TimeSpan.FromHours(12));
                        }
                    }
                }

                return fromCache;
            }
        }

        

        public bool Clear(string name)
        {
            cache.Remove(name);

            return true;
        }

        public T Get<T>(string cacheKey) where T : class
        {
            object cookies;

            if (!cache.TryGetValue(cacheKey, out cookies))
                return null;

            return cookies as T;
        }

        public void Set(string cacheKey, object value, TimeSpan? timeSpan)
        {
            if (timeSpan == null) { timeSpan = TimeSpan.FromMinutes(180); }

            cache.Set(cacheKey, value, Convert.ToInt32(timeSpan.Value.TotalMinutes));
        }
    }
}

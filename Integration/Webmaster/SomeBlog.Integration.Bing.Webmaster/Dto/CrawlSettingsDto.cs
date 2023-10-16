using BingWebmasterService;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class CrawlSettingsDto
    {
        public bool CrawlBoostAvailable;
        public bool CrawlBoostEnabled;
        public byte[] CrawlRate;

        public static CrawlSettingsDto From(CrawlSettings obj)
        {
            return new CrawlSettingsDto()
            {
                CrawlBoostAvailable = obj.CrawlBoostAvailable,
                CrawlBoostEnabled = obj.CrawlBoostEnabled,
                CrawlRate = obj.CrawlRate
            };
        }
    }
}

using BingWebmasterService;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class UrlSubmissionQuotaDto
    {
        public int DailyQuota;
        public int MonthlyQuota;

        public static UrlSubmissionQuotaDto From(UrlSubmissionQuota obj)
        {
            return new UrlSubmissionQuotaDto()
            {
               DailyQuota = obj.DailyQuota,
               MonthlyQuota = obj.MonthlyQuota
            };
        }
    }
}

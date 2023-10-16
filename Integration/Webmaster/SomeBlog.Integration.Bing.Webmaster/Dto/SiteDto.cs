using BingWebmasterService;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class SiteDto
    {
        public string AuthenticationCode;
        public string DnsVerificationCode;
        public bool IsVerified;
        public string Url;

        public static SiteDto From(Site x)
        {
            return new SiteDto()
            {
                AuthenticationCode = x.AuthenticationCode,
                DnsVerificationCode = x.DnsVerificationCode,
                IsVerified = x.IsVerified,
                Url = x.Url
            };
        }

        public static List<SiteDto> From(List<Site> x)
        {
            return x.Select(y => From(y)).ToList();
        }
    }
}

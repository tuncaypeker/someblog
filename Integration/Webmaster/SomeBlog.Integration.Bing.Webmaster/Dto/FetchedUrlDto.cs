using BingWebmasterService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class FetchedUrlDto
    {
        public DateTime Date;
        public bool Expired;
        public bool Fetched;
        public string Url;

        public static FetchedUrlDto From(FetchedUrl obj)
        {
            return new FetchedUrlDto()
            {
               Date = obj.Date,
               Expired = obj.Expired,
               Fetched = obj.Fetched,
               Url = obj.Url
            };
        }

        public static List<FetchedUrlDto> From(List<FetchedUrl> objs)
        {
            return objs.Select(x => From(x)).ToList();
        }
    }
}

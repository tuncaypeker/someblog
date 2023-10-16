using BingWebmasterService;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class KeywordDto
    {
        public long BroadImpressions;
        public long Impressions;
        public string Query;

        public static KeywordDto From(Keyword obj)
        {
            return new KeywordDto()
            {
                BroadImpressions = obj.BroadImpressions,
                Impressions = obj.Impressions,
                Query = obj.Query
            };
        }

        public static List<KeywordDto> From(List<Keyword> objs)
        {
            return objs.Select(x => From(x)).ToList();
        }
    }
}

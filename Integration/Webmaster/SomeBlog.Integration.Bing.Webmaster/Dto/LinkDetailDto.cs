using BingWebmasterService;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class LinkDetailsDto
    {
        private List<LinkDetailDto> Details;
        private int TotalPages;

        public static LinkDetailsDto From(LinkDetails x)
        {
            return new LinkDetailsDto()
            {
                TotalPages = x.TotalPages,
                Details = LinkDetailDto.From(x.Details)
            };
        }
    }

    public class LinkDetailDto
    {
        public string AnchorText;
        public string Url;

        public static LinkDetailDto From(LinkDetail x)
        {
            return new LinkDetailDto()
            {
                Url = x.Url,
                AnchorText = x.AnchorText
            };
        }

        public static List<LinkDetailDto> From(List<LinkDetail> x)
        {
            return x.Select(y => From(y)).ToList();
        }
    }
}

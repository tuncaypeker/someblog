using System;
using System.Collections.Generic;

namespace SomeBlog.Integration.Yandex.Webmaster.Dto
{
    public class SitemapByUser
    {
        public string sitemap_id { get; set; }
        public string sitemap_url { get; set; }
        public DateTime added_date { get; set; }
    }

    public class GetSitemapsByUserResponseDto
    {
        public List<SitemapByUser> sitemaps { get; set; }
        public int count { get; set; }
        public string error_code { get; set; }
    }
}

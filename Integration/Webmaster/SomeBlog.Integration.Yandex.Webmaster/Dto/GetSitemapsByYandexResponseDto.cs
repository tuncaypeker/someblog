using System;
using System.Collections.Generic;

namespace SomeBlog.Integration.Yandex.Webmaster.Dto
{
    public class SitemapByYandex
    {
        public string sitemap_id { get; set; }
        public string sitemap_url { get; set; }
        public DateTime last_access_date { get; set; }
        public int errors_count { get; set; }
        public int urls_count { get; set; }
        public int children_count { get; set; }

        /// <summary>
        /// SITEMAP
        /// INDEX_SITEMAP
        /// </summary>
        public string sitemap_type { get; set; }
    }

    public class GetSitemapsByYandexResponseDto
    {
        public string error_code { get; set; }
        public List<SitemapByYandex> sitemaps { get; set; }
    }
}

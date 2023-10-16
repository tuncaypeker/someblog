namespace SomeBlog.Integration.Yandex.Webmaster.Dto
{
    public class GetSiteStatisticsResponseDto
    {
        public int sqi { get; set; }
        public int excluded_pages_count { get; set; }
        public int searchable_pages_count { get; set; }
        public SiteProblems site_problems { get; set; }

        public class SiteProblems
        {
            public int POSSIBLE_PROBLEM { get; set; }
        }
    }
}

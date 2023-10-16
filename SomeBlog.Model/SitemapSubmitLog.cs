namespace SomeBlog.Model
{
    public class SitemapSubmitLog : Core.ModelBase
    {
        public int BlogId { get; set; }
        public string SitemapPath { get; set; }
        public System.DateTime SubmitDate { get; set; }
        
        /// <summary>
        /// 1- Search Console
        /// 2- Bing Webmaster
        /// 3- Yandex Webmaster
        /// </summary>
        public int Platform { get; set; }

        public bool IsSubmitted { get; set; }
        public string Message { get; set; }
    }
}

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class IndexNowDto
    {
        public string host { get; set; }
        public string key { get; set; }
        public string keyLocation { get; set; }
        public string[] urlList { get; set; }
    }
}

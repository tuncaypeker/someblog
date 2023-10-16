namespace SomeBlog.Model
{
    public class Theme : Core.ModelBase
    {
        public string Name { get; set; }
        public int HomeFeaturedCount { get; set; }
        public string LastVersionName { get; set; }
        public string LastVersionHash { get; set; }
        public string Thumbnail { get; set; }
    }
}

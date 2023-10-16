namespace SomeBlog.Model
{
    public class ScraperReplaceRule : Core.ModelBase
    {
        public int ScraperId { get; set; }
        public string ReplaceWhat { get; set; }
        public string ReplaceWith { get; set; }
    }
}

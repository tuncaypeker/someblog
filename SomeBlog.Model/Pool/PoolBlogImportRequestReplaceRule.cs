namespace SomeBlog.Model
{
    public class PoolBlogImportRequestReplaceRule : Core.ModelBase
    {
        public int PoolBlogImportRequestId { get; set; }
        public string ReplaceWhat { get; set; }
        public string ReplaceWith { get; set; }
    }
}

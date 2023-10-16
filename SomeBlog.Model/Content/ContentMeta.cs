namespace SomeBlog.Model
{
    public class ContentMeta : Core.ModelBase
    {
        public int ContentId { get; set; }
        public int MetaId { get; set; }
        public string Value { get; set; }
    }
}

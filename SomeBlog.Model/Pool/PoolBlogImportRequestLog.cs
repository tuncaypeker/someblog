namespace SomeBlog.Model
{
    using System;

    public class PoolBlogImportRequestLog : Core.ModelBase
    {
        public int PoolBlogId { get; set; }
        public int PoolContentId { get; set; }
        public int BlogId { get; set; }
        public int ContentId { get; set; }
        public DateTime Created { get; set; }
        public DateTime PublishDate { get; set; }
        public int PoolBlogImportRequestId { get; set; }
    }
}

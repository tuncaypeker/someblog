namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;

    public class PoolContentComment : ModelBase
    {
        public int PoolContentId { get; set; }
        public int PoolBlogId { get; set; }
        public string SiteKeyId { get; set; }
        public string SiteContentKeyId { get; set; }
        public int ParentId { get; set; }
        public string Fullname { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
    }
}

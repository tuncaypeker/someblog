namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;

    public class PoolContentUsage : ModelBase
    {
        public int PoolContentId { get; set; }
        public int BlogId { get; set; }
        public int ContentId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

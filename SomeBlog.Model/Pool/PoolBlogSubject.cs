namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;

    public class PoolBlogSubject : ModelBase
    {
        public int SubjectId { get; set; }
        public int PoolBlogId { get; set; }
    }
}

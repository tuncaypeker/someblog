namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;

    public class PoolTag : ModelBase
    {
        public int PoolBlogId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}

namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;

    /// <summary>
    /// Site tarafından gelen kategoriler bunun icerisne girecek
    /// </summary>
    public class PoolCategory : ModelBase
    {
        public int PoolBlogId { get; set; }
        public int ParentId { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }
    }
}

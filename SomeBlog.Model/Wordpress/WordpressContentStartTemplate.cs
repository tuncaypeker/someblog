namespace SomeBlog.Model
{
    /// <summary>
    /// Bot, BlogImport gibi dışardan gelen içeriklerin başına eklenmek üzere kullanılabilecek 
    /// sablonlar
    /// </summary>
    public class WordpressContentStartTemplate : Core.ModelBase
    {
        public int WordpressSiteId { get; set; }

        /// <summary>
        /// {blog} {pagetitle} {category}
        /// </summary>
        public string Template { get; set; }
    }
}

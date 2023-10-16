namespace SomeBlog.Model
{
    /// <summary>
    /// Bot, BlogImport gibi dışardan gelen içeriklerin başına eklenmek üzere kullanılabilecek 
    /// sablonlar
    /// </summary>
    public class ContentStartTemplate : Core.ModelBase
    {
        public int BlogId { get; set; }

        public int ContentStartTemplateCategoryId { get; set; }

        /// <summary>
        /// {blog} {pagetitle} {category}
        /// </summary>
        public string Template { get; set; }
    }
}

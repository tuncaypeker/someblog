namespace SomeBlog.Model
{
    using System.Collections.Generic;

    public class Page : Core.ModelBase
    {
        public int BlogId { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

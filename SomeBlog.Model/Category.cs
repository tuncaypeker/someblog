namespace SomeBlog.Model
{
    using System.Collections.Generic;

    public class Category : Core.ModelBase
    {
        public Category(int blogId)
        {
            BlogId = blogId;
        }

        public Category()
        {

        }

        public int ParentId { get; set; }
        public int BlogId { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }

        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }

        public string ImagePath { get; set; }
        public int Hit { get; set; }
    }
}

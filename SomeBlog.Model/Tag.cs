namespace SomeBlog.Model
{
    using System.Collections.Generic;

    public class Tag : Core.ModelBase
    {
        public Tag(int blogId)
        {
            BlogId = blogId;
        }

        public Tag()
        {

        }

        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Hit { get; set; }

        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
    }
}

namespace SomeBlog.Model
{
    using System;

    public class ContentUpdateHistory : Core.ModelBase
    {
        public ContentUpdateHistory()
        {
            Excerpt = "";
            PageTitle = "";
            PageDescription = "";
            MetaTitle = "";
            MetaDescription = "";
            Text = "";
            Slug = "";
        }

        public int ContentId { get; set; }
        public DateTime CreateDate { get; set; }
        
        public string Excerpt { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Text { get; set; }
        public string Slug { get; set; }
        public int UserId { get; set; }
    }
}

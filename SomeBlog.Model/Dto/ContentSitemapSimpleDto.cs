using System;

namespace SomeBlog.Model.Dto
{
    public class ContentSitemapSimpleDto
    {
        public string Slug { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

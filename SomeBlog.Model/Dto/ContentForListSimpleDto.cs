using System;

namespace SomeBlog.Model.Dto
{
    public class ContentForListSimpleDto
    {
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string Slug { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string FeaturedMediaUrl { get; set; }
        public string FeaturedMediaAlt { get; set; }
        public string FeaturedMediaTitle { get; set; }
        public bool FeaturedMediaHasWebp { get; set; }
        public bool FeaturedMediaHasAvif { get; set; }
        public int FeaturedMediaWidth { get; set; }
        public int FeaturedMediaHeight { get; set; }
        public int FirstCategoryId { get; set; }
    }
}

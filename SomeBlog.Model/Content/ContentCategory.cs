namespace SomeBlog.Model
{
    using System.Collections.Generic;

    public class ContentCategory : Core.ModelBase
    {
        public ContentCategory()
        {
        }

        public ContentCategory(int contentId, int categoryId)
        {
            ContentId = contentId;
            CategoryId = categoryId;
        }

        public int ContentId { get; set; }
        public int CategoryId { get; set; }
    }
}

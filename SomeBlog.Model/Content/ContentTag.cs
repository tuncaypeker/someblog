namespace SomeBlog.Model
{
    public class ContentTag : Core.ModelBase
    {
        public ContentTag()
        {
        }

        public ContentTag(int contentId, int tagId)
        {
            ContentId = contentId;
            TagId = tagId;
        }

        public int ContentId { get; set; }
        public int TagId { get; set; }
    }
}

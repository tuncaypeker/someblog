namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;

    public class PoolContentTag : ModelBase
    {
        public PoolContentTag()
        {
        }

        public PoolContentTag(int contentId, int tagId)
        {
            PoolContentId = contentId;
            PoolTagId = tagId;
        }

        public int PoolContentId { get; set; }
        public int PoolTagId { get; set; }
    }
}

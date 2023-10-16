namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;

    public class PoolContentCategory : ModelBase
    {
        public PoolContentCategory()
        {
        }

        public PoolContentCategory(int contentId, int categoryId)
        {
            PoolContentId = contentId;
            PoolCategoryId = categoryId;
        }

        public int PoolContentId { get; set; }
        public int PoolCategoryId { get; set; }
    }
}

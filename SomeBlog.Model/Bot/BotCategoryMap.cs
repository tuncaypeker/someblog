namespace SomeBlog.Model
{
    public class BotCategoryMap : Core.ModelBase
    {
        public int BotId { get; set; }
        public int RemoteCategoryId { get; set; }
        public int LocalCategoryId { get; set; }
    }
}

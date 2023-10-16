namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;

    public class PoolBlogUpdateLogItem : ModelBase
    {
        public PoolBlogUpdateLogItem()
        {

        }

        public PoolBlogUpdateLogItem(System.DateTime date, string description)
        {
            Date = date;
            Description = description;
        }

        public int PoolBlogUpdateLogId { get; set; }
        public string Description { get; set; }
        public System.DateTime Date { get; set; }
    }
}

namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;

    public class PoolBlogUpdateLog : ModelBase
    {
        public int PoolBlogId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsFinished { get; set; }
        public string Description { get; set; }
    }
}

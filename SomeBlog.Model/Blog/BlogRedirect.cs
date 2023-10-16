namespace SomeBlog.Model
{
    public class BlogRedirect : Core.ModelBase
    {
        public int BlogId { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// https://www.hazirbilgi.net/uzmandan-9-soruda-asi-hakkinda-her-sey.html/feed
        /// Route: {path}/feed
        /// </summary>
        public string Route { get; set; } 
    }
}

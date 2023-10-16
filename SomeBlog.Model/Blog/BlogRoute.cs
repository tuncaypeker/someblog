namespace SomeBlog.Model
{
    public class BlogRoute : Core.ModelBase
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        /// <summary>
        /// : ile ayir
        /// </summary>
        public string Parameter { get; set; }
        public int Order { get; set; }
    }
}

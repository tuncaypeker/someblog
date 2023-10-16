using System.Collections.Generic;

namespace SomeBlog.Wordpress.Feed.Rdf.Model
{
    public class Post
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string Summary { get; set; }
        public List<string> Categories { get; set; }
        public string Content { get; set; }
        public System.DateTime PublishDate { get; set; }
        public string Link { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace SomeBlog.Blogspot.Api.Dto
{
    public class PostItem
    {
        public string kind { get; set; }
        public string id { get; set; }
        public DateTime published { get; set; }
        public DateTime updated { get; set; }
        public string url { get; set; }
        public string selfLink { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public List<string> labels { get; set; }
        public string etag { get; set; }
        public Author author { get; set; }
    }

    public class Author
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string url { get; set; }
    }
}

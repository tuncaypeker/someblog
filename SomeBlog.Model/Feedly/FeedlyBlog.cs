using System;
using System.Collections.Generic;

namespace SomeBlog.Model
{
    public class FeedlyBlog : Core.ModelBase
    {
        public string ContentType { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public decimal LeoScore { get; set; }
        public int Subscribers { get; set; }
        public string Title { get; set; }
        public DateTime Updated { get; set; }
        public string Path { get; set; }
        public string FeedPath { get; set; }

        public bool CheckLater { get; set; } 

        public virtual List<FeedlyBlogTopic> FeedlyBlogTopics { get; set; }
    }
}

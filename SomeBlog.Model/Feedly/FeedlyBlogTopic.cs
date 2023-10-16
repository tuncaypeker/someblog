using System;

namespace SomeBlog.Model
{
    public class FeedlyBlogTopic : Core.ModelBase
    {
        public int FeedlyTopicId { get; set; }
        public int FeedlyBlogId { get; set; }
    }
}

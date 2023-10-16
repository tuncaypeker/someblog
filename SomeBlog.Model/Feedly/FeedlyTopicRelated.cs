namespace SomeBlog.Model
{
    public class FeedlyTopicRelated : Core.ModelBase
    {
        public int FeedlyTopicId { get; set; }
        public int RelatedTopicId { get; set; }
    }
}

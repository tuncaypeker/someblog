namespace SomeBlog.Model
{
    public class BlogMailHistory : Core.ModelBase
    {
        public int BlogMailId { get; set; }
        public string From { get; set; }
        public string MessageId { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
    }
}

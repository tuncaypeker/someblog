namespace SomeBlog.Model
{
    public class BotReplaceRule : Core.ModelBase
    {
        public int BotId { get; set; }
        public string ReplaceWhat { get; set; }
        public string ReplaceWith { get; set; }
    }
}

namespace SomeBlog.Model
{
    public class BotRemoteTag : Core.ModelBase
    {
        public int BotId { get; set; }
        public int TagId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
    }
}

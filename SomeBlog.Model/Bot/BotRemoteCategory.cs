namespace SomeBlog.Model
{
    public class BotRemoteCategory : Core.ModelBase
    {
        public int BotId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
    }
}

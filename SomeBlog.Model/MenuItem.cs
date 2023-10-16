namespace SomeBlog.Model
{
    public class MenuItem : Core.ModelBase
    {
        public int MenuId { get; set; }
        public int ParentId { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
    }
}

namespace SomeBlog.Model
{
    public class Menu : Core.ModelBase
    {
        public int BlogId { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
    }
}

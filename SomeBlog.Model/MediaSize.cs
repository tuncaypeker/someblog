namespace SomeBlog.Model
{
    public class MediaSize : Core.ModelBase
    {
        public int BlogId { get; set; }
        public string Key { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Description { get; set; }
    }
}

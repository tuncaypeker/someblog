namespace SomeBlog.Model
{
    public class Wiki : Core.ModelBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Source { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}

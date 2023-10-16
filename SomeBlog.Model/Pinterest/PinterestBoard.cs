namespace SomeBlog.Model
{
    public class PinterestBoard : Core.ModelBase
    {
        public int AccountId { get; set; }
        public string BoardId { get; set; }
        public string ImageCoverUrl { get; set; }
        public string Name { get; set; }
    }
}

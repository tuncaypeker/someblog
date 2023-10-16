namespace SomeBlog.Model
{
    public class PinterestPoolPost : Core.ModelBase
    {
        public string PinId { get; set; }
        public int PoolAccountId { get; set; }
        public int PoolBoardId { get; set; }
        public string LocalImagePath { get; set; }
        public string RemoteImagePath { get; set; }
        public string Title { get; set; }
        public string Domain { get; set; }
        public string Description { get; set; }
        public int RepinCount { get; set; }
        public string Link { get; set; }
        public string Type { get; set; }
        public bool IsRepin { get; set; }
        public string LocalVideoPath { get; set; }
        public string RemoteVideoPath { get; set; }
        public bool IsVideo { get; set; }
    }
}

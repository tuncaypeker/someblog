namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;

    public class PoolMedia : ModelBase
    {
        public PoolMedia()
        {
            HasFeatured = false;
            HasDownloaded = false;
            LocalPath = "";
        }

        public int PoolContentId { get; set; }

        public string RemotePath { get; set; }
        public string LocalPath { get; set; }
        public bool HasDownloaded { get; set; }
        public bool HasError { get; set; }
        public int StatusCode { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Alt { get; set; }
        public bool HasFeatured { get; set; }
    }
}

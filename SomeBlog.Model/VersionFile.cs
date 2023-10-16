namespace SomeBlog.Model
{
    public class VersionFile : Core.ModelBase
    {
        public string FilePath { get; set; }
        public string Hash { get; set; }
        public int VersionId { get; set; }
    }
}

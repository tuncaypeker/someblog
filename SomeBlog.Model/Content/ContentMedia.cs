namespace SomeBlog.Model
{
    public class ContentMedia : Core.ModelBase
    {
        public int ContentId { get; set; }
        
        //media tablosunda yoksa -1 olarak isaretle
        public int MediaId { get; set; }

        public string Path { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsExternal { get; set; }
    }
}

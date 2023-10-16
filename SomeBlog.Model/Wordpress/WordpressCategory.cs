namespace SomeBlog.Model
{
    public class WordpressCategory : Core.ModelBase
    {
        public int WordpressSiteId { get; set; }
        
        /// <summary>
        /// Sitede bulunan id'ye karsilik gelir
        /// </summary>
        public int RemoteCategoryId { get; set; }
        public string Name { get; set; }
    }
}

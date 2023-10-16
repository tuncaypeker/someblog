namespace SomeBlog.Model
{
    public class WordpressSiteBotCategoryMap : Core.ModelBase
    {
        public int WordpressSiteBotId { get; set; }

        /// <summary>
        /// Bot'un ait oldugu wordpress site'deki CategoryId'sidir
        /// </summary>
        public int RemoteCategoryId { get; set; }

        /// <summary>
        /// WordpressCategory modeline ait, bizim db'deki Id Primary Key'dir
        /// </summary>
        public int LocalCategoryId { get; set; }
    }
}

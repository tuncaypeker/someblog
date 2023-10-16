using System.Collections.Generic;

namespace SomeBlog.Integration.Bing.SearchImage.Dto
{
    public class ImageResultDto
    {
        public List<ImageDto> Images { get; set; }
        public string Message { get; set; }
        public bool IsSucceed { get; set; }
    }

    public class ImageDto
    {
        public string sid { get; set; }
        public string cturl { get; set; }
        public string cid { get; set; }

        /// <summary>
        /// Page url, resmin oldugu sayfa
        /// </summary>
        public string purl { get; set; }

        /// <summary>
        /// sitedeki adresi
        /// </summary>
        public string murl { get; set; }

        /// <summary>
        /// bingdeki thumb url
        /// </summary>
        public string turl { get; set; }
        public string md5 { get; set; }
        public string shkey { get; set; }
        public string t { get; set; }
        public string mid { get; set; }
        public string desc { get; set; }
    }
}

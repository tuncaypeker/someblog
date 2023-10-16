namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;

    public class PoolContentTranslate : ModelBase
    {
        public int PoolBlogId { get; set; }
        public int PoolContentId { get; set; }
        public int PoolLanguageId { get; set; }

        /// <summary>
        /// 1- Google Translate
        /// 2- Google Cloud Api
        /// 3- Microsoft Bing
        /// 4- Yandex Translate
        /// 5- DeeplTranslate
        /// </summary>
        public int Source { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

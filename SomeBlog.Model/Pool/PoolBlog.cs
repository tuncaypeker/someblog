namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;
    using System.Collections.Generic;

    public class PoolBlog : ModelBase
    {
        public PoolBlog()
        {
            //WpJson olarak kabul edelim
            FeedType = 1;
            ApiPerPage = 100;
            IsAlexaRelatedChecked = false;
        }

        public string Name { get; set; }
        public string Path { get; set; }

        /// <summary>
        /// Blogspot siteler icin kullanilan deger
        /// </summary>
        public string BlogId { get; set; }

        /// <summary>
        /// Sitenin ne ile ilgili oldugu hangi alanda yazılar yayınladıgı ile ilgili
        /// </summary>
        public string About { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string IpAdress { get; set; }
        public string AdsensePublisherId { get; set; }
        public int AlexaRank { get; set; }

        public int PoolLanguageId { get; set; }

        public DateTime LastUpdate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ImportDate { get; set; }

        public bool HasFirstImportDone { get; set; }
        public bool ShouldTranslateContents { get; set; }
        public bool ShouldInsertNewContents { get; set; }
        public bool IgnoreTags { get; set; }

        /// <summary>
        /// En son hangi sayfayi almisiz iceri bilgisi
        /// </summary>
        public int FirstImportLastPage { get; set; }

        /// <summary>
        /// Max bu siteden kaçıncı sayfaya kadar ilerlesin bilgisi
        /// </summary>
        public int MaxImportLastPage { get; set; }

        /// <summary>
        /// split category ids by comma
        /// </summary>
        public string CategoryFilter { get; set; }

        public virtual List<PoolBlogSubject> PoolBlogSubjects { get; set; }

        /// <summary>
        /// 1- WpJson
        /// 2- FeedRdf
        /// 3- Blogspot
        /// </summary>
        public int FeedType { get; set; }

        /// <summary>
        /// Bazı sitelerde cok ilginc per_page parametresi 100 gidince cevap vermiyor
        /// ornek olarak bu site https://www.cssscript.com//wp-json/wp/v2/posts?per_page=10&page=1
        /// </summary>
        public int ApiPerPage { get; set; }

        public bool IsBlogRetired { get; set; }
        public bool IsWpJsonRetired { get; set; }
        public bool IsAlexaRelatedChecked { get; set; }
    }
}

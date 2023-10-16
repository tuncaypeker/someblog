using System;

namespace SomeBlog.Model
{
    public class PoolBlogImportRequest : Core.ModelBase
    {
        public int PoolBlogId { get; set; }
        public int BlogId { get; set; }
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
        public bool AddPostAsActive { get; set; }
        
        /// <summary>
        /// sadece içerdeki linkleri kaldırır ve anchor içindeki html'e dokunmaz
        /// </summary>
        public bool RemoveAnchors { get; set; }
        public bool RemoveAnchorTagOnly { get; set; }

        public int AddPostHourSpacing { get; set; }
        public int PostPublishHour { get; set; }
        public int PostPublishMinute { get; set; }
        public int PostPublishCountNow { get; set; }

        /// <summary>
        /// Date
        /// Title
        /// Id
        /// SiteKeyId
        /// Rasgele
        /// </summary>
        public string OrderContentsBy { get; set; }

        public string PostCategoryMapping { get; set; }
        public bool HasDone { get; set; }
        public bool PrependContentTemplate { get; set; }

        /// <summary>
        /// Hangi dilde translate'i eklenmek isteniyor onu belirler
        /// eger bos ise orjinal icerik eklenir
        /// bknz. importtoblogbot
        /// </summary>
        public int PoolLanguageId { get; set; }
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// Bu id'ye sahip poolContent'ler ignore edilir
        /// </summary>
        public string ExcludedIds { get; set; }

        /// <summary>
        /// Kelime sayısı hesaplanır ve bu kelime sayısından daha az kelime varsa eklenmez
        /// </summary>
        public int MinWordCount { get; set; }

        /// <summary>
        /// Remove Attrs from tags, comma seperated
        /// Content Edit ekranındaki gibi, html içindeki class, id, style gibi attr'leri kaldirir
        /// </summary>
        public string RemoveAttributes { get; set; }

        /// <summary>
        /// Secilen Xpath ile bulunan etiketlerin sıralamasını karıştırır
        /// </summary>
        public string MixXpath { get; set; }

         /// <summary>
        /// Eger sayfa basliginda, meta başlıkta, sayfa açıklamasında ya da meta açıklamasında 
        /// bu , ile ayrılan kelimelerden biri denk gelirse
        /// o Postu atla ve ekleme
        /// </summary>
        public string StopWords { get; set; }

        /// <summary>
        /// Title'a buraya bir deger eklendi ise ekler ve slug'ı buna gore generate eder
        /// , ile ayırarak birden fazla deger girilebilir
        /// </summary>
        public string AppendToTitleText { get; set; }

          /// <summary>
        /// Sadece bu kategorilerden icerik alir
        /// </summary>
        public string CategoryIds { get; set; }

        /// <summary>
        /// Özellikle bu kategorilerden icerik alma
        /// </summary>
        public string ExcludeCategoryIds { get; set; }

        //eger kategori mapping bulamazsa bu kategoriye eklesin
        public int DefaultCategoryId { get; set; }
    }
}

using System;

namespace SomeBlog.Model
{
    public class Bot : Core.ModelBase
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// icerik alinacak wordpress sitenin ana adresi https://evdekorasyonlari.net gibi
        /// </summary>
        public string WordpressSitePath { get; set; }

        //* * * * * 2 gibi cron interval
        public string CronInterval { get; set; }

        /// <summary>
        /// Son kac icerik ile calismamiz gerekiyor
        /// </summary>
        public int LastPostCount { get; set; }

        /// <summary>
        /// Metin icerisinde bulunan linkleri kaldirayim mi
        /// sadece içerdeki linkleri kaldırır ve anchor içindeki html'e dokunmaz
        /// </summary>
        public bool RemoveAnchors { get; set; }
        public bool RemoveAnchorTagOnly { get; set; }

        /// <summary>
        /// span etiketini kaldırır, icerisindeki html ya da text'e dokunmaz
        /// </summary>
        public bool RemoveSpanTags { get; set; }

        /// <summary>
        /// <p></p>, <div></div>, <span></span> etiketleri icin bos olanlari temizler
        /// </summary>
        public bool RemoveEmptyTags { get; set; }

        /// <summary>
        /// Eger sayfa basliginda, meta başlıkta, sayfa açıklamasında ya da meta açıklamasında 
        /// bu , ile ayrılan kelimelerden biri denk gelirse
        /// o Postu atla ve ekleme
        /// </summary>
        public string StopWords { get; set; }

        /// <summary>
        /// burasi sadece turkce icin mi calisiyor bu durumda
        /// </summary>
        public bool SpinWithOtoSpin { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        /// Tag'ler sisteme eklenmez
        /// </summary>
        public bool IgnoreTags { get; set; }

        /// <summary>
        /// Post sisteme aktif olarak eklenir ve yayına alınır
        /// </summary>
        public bool AddPostAsActive { get; set; }

        /// <summary>
        /// Sadece bu kategorilerden icerik alir
        /// </summary>
        public string CategoryIds { get; set; }

        /// <summary>
        /// Özellikle bu kategorilerden icerik alma
        /// </summary>
        public string ExcludeCategoryIds { get; set; }

        /// <summary>
        /// Remove Attrs from tags, comma seperated
        /// Content Edit ekranındaki gibi, html içindeki class, id, style gibi attr'leri kaldirir
        /// </summary>
        public string RemoveAttributes { get; set; }

        public bool DoTranslateContent { get; set; }
        public string SourceLangCode { get; set; }
        public string TargetLangCode { get; set; }

        //eger kategori mapping bulamazsa bu kategoriye eklesin
        public int DefaultCategoryId { get; set; }

        public DateTime LastHit { get; set; }

        //ozellikle eklemedik istedigin sablon kategorisi var mi
        //ornek olarak bu sarki sozleri kategorisi ise sarki sozleri template'leri
        //secilmez ise kategoriye bagli olmayanlardan birini alir
        public int ContentStartTemplateCategoryId { get; set; }
    }
}

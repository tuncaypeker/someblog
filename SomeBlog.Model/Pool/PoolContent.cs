namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;
    using System.Collections.Generic;

    public class PoolContent : ModelBase
    {
        public int PoolBlogId { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public string SiteKeyId { get; set; }

        /// <summary>
        /// date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// modified
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// slug
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// title:rendered
        /// </summary>
        public string Title { get; set; }
        public string TitleTR { get; set; }
        public string TitleEN { get; set; }

        /// <summary>
        /// content:rendered
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Excerpt
        /// </summary>
        public string Excerpt { get; set; }

        /// <summary>
        /// author
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Sistem tarafina eklenme tarihi
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Sistem tarafinda guncelleme tarihi
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Ekstra bir durum yok ise blog'dan almali
        /// </summary>
        public int PoolLanguageId { get; set; }

        /// <summary>
        /// slugdan uretilmiyor, sonradan ekledik
        /// </summary>
        public string Link { get; set; }

        public bool IsDeleted { get; set; }

        /// <summary>
        /// Translate botu, bazı içerikleri çeviremiyor ve bunun sonucunda tekrar tekrar denemeye calisiyor
        /// Bu flag bu sürekli deneme islemini engellemek icin eklendi
        /// Eger transalte edilemez ise bu flag isaretleniyor ve translate botu bu flag isaretlenmis icerikleri cevirmeye calısmıyor
        /// </summary>
        public bool CantTranslated { get; set; }

        /// <summary>
        /// PoolMedia tablosu sonradan eklendi ve bunun oncesinde poolcontent'e 
        /// eklenen icerikler icin bot yazildi, bot bu flag'i kullanarak islenmemis contentler icin 
        /// medialari isler
        /// </summary>
        public bool HasMediaProcessed { get; set; }

        /// <summary>
        /// Bu content'in commentlerini almaya calistin mi
        /// </summary>
        public bool HasCommentProcessed { get; set; }

        public virtual List<Model.PoolContentCategory> PoolContentCategories { get; set; }
    }
}

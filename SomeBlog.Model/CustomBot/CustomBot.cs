namespace SomeBlog.Model
{
    public class CustomBot : Core.ModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Eger sayfa basliginda, meta başlıkta, sayfa açıklamasında ya da meta açıklamasında 
        /// bu , ile ayrılan kelimelerden biri denk gelirse
        /// o Postu atla ve ekleme
        /// </summary>
        public string StopWords { get; set; }
        
        public bool IsActive { get; set; }

         /// <summary>
        /// Post sisteme aktif olarak eklenir ve yayına alınır
        /// </summary>
        public bool AddPostAsActive { get; set; }
    }
}

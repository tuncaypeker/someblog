namespace SomeBlog.Model
{
    public class AlexaSiteRelated : Core.ModelBase
    {
        public int AlexaSiteId { get; set; }
        public string Related { get; set; }

        /// <summary>
        /// Bu ozellik havuz bloglarının related'larına bakarken kullanildi
        /// sadece havuzdaki bloglar icin haswpjson bakildi
        /// Tamamını kapsamıyor ve yanıltabilir
        /// </summary>
        public bool HasWpjson { get; set; }
    }
}

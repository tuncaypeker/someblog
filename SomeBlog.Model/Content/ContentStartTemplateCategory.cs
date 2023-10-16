namespace SomeBlog.Model
{
    /// <summary>
    /// Bazı durumlarda bot veya scraper'lar ozel start template'lere ihtiyac duyabiliyor
    /// Ornek olarak esozlerim, sarki sozleri botlari kendisine ozel start template eklemek durumunda, cunku diger template'ler ozlu sozler vs
    /// ivin kullaniliyor ve sarki sozuun basina eklendiginde sacma oluyor
    /// sablonlar
    /// </summary>
    public class ContentStartTemplateCategory : Core.ModelBase
    {
        public int BlogId { get; set; }

        //sarki-sozleri
        public string Name { get; set; }
    }
}

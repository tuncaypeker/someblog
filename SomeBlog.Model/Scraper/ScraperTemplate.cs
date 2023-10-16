namespace SomeBlog.Model
{
    /// <summary>
    /// Google Trends'den site çekerken bu sablonlari kullaniyoruz her seferinde tekrar girmeyelim die
    /// GoogleTrends > Realttime > Yandex Preview > Inject 
    /// </summary>
    public class ScraperTemplate : Core.ModelBase
    {
        public string BaseDomain { get; set; }
        public string TitleXPath { get; set; }
        public string ContentXPath { get; set; }
        public string ImageXPath { get; set; }
        
    }
}

namespace SomeBlog.Model
{
    public class ScraperRemoveRule : Core.ModelBase
    {
        public int ScraperId { get; set; }
        public string Value { get; set; }

        /// <summary>
        ///  - dekorstilleri.com
        ///  dekorstilleri.com iki tane rule olsa bu rulelardan once ikincisi calisirsa " - " ortada kalir
        ///  bu yuzden order onemli
        /// </summary>
        public int Order { get; set; }

        public bool IsXPath { get; set; }
    }
}

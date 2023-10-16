namespace SomeBlog.Model.Enums
{
    public enum SeoTools
    {
        Google_Search_Console = 1,
        Semrush = 2,
        MozPro = 3,
        Ahrefs = 4,
        Yandex_Webmaster = 5,
        Bing_Webmaster = 6
    }

    public class SeoToolsEnumHelpers
    {
        public static string ToFiendlyName(int value)
        {
            switch (value) {
                case 1: return "Google Search Console";
                case 2: return "Semrush";
                case 3: return "Mozpro";
                case 4: return "Ahrefs";
                case 5: return "Yandex Webmaster";
                case 6: return "Bing Webmaster";
                case 7: return "Keyword Everywhere";
                default: return value.ToString();
            }
        }
    }

}

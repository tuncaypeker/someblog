namespace SomeBlog.Model.Enums
{
    public enum ContentAddType
    {
        FormAdd = 1,
        BotAdd = 2,
        PoolContentInject = 3,
        PoolContentImport = 4,
        PAAContentGenerate = 5
    }

    public static class ContentAddTypeEnumHelpers
    {
        public static string ToFriendlyName(this ContentAddType value)
        {
            switch (value) {
                case ContentAddType.FormAdd: return "Form ile Eklenmiş";
                case ContentAddType.BotAdd: return "Bot ile Eklenmiş";
                case ContentAddType.PoolContentInject: return "Havuz içeriği Inject Edilmiş";
                case ContentAddType.PoolContentImport: return "Havuz içeriği Import Edilmiş";
                case ContentAddType.PAAContentGenerate: return "PAA İçerik Generate Edilmiş";
                default: return "Bilinmiyor";
            }
        }
    }
}

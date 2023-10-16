using DeepL;
using System;

namespace SomeBlog.Translate.Deepl
{
    public class Translator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceLang">tr</param>
        /// <param name="targetLang">en</param>
        /// <param name="text">Merhaba</param>
        public static void Translate(string sourceLang, string targetLang, string text)
        {
            using (DeepLClient client = new DeepLClient("8ad12094-a26e-dc2a-2e3b-abad7aa022c7", useFreeApi: false))
            {
                try
                {
                    Translation translation = client.TranslateAsync(
                       text,
                       sourceLang,
                       targetLang
                    ).Result;

                    Console.WriteLine(translation.DetectedSourceLanguage);
                    Console.WriteLine(translation.Text);
                }
                catch (DeepLException exception)
                {
                    Console.WriteLine($"An error occurred: {exception.Message}");
                }
            }
        }
    }
}

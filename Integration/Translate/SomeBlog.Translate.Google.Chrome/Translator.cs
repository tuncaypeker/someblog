using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Text;
using System.Web;

namespace SomeBlog.Translate.Google.Chrome
{
    public class Translator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceLang">tr</param>
        /// <param name="targetLang">en</param>
        /// <param name="text">Merhaba</param>
        public string Translate(string sourceLang, string targetLang, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text) || text == "\n")
                    return text;

                //https://www.labnol.org/code/19909-google-translate-api
                var url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
                    + sourceLang + "&tl=" + targetLang + "&dt=t&q=" + HttpUtility.UrlPathEncode(text);

                StringBuilder translatedText = new StringBuilder();

                using (var client = new WebClient())
                {
                    var jsonStr = client.DownloadString(url);
                    var json = JArray.Parse(jsonStr);

                    var translatedArr = JArray.Parse(json[0].ToString());
                    foreach (var translatedItem in translatedArr)
                        translatedText.Append(translatedItem[0].ToString());
                }

                return translatedText.ToString();
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public string TranslateHtml(string html, string sourceLang, string targetLang)
        {
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);
            TranslateHtmlNodeCollection(htmlDocument.DocumentNode.ChildNodes, sourceLang: sourceLang, targetLang: targetLang);
            return htmlDocument.DocumentNode.OuterHtml;
        }

        private void TranslateHtmlNodeCollection(HtmlAgilityPack.HtmlNodeCollection nodeCollection, string sourceLang, string targetLang)
        {
            if (nodeCollection == null || nodeCollection.Count == 0)
                return;

            foreach (var node in nodeCollection)
            {
                if (node.Name == "#text" && node.InnerText != "\n") {
                    var text = node.InnerText;

                    if (text.StartsWith("#"))
                        text = text.TrimStart('#');

                    node.InnerHtml = Translate(sourceLang.ToLower(), targetLang, text);
                }

                TranslateHtmlNodeCollection(node.ChildNodes, sourceLang, targetLang);
            }
        }
    }
}

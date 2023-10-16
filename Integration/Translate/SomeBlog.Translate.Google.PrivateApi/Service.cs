using Newtonsoft.Json;
using RestSharp;
using System;

namespace SomeBlog.Translate.Google.PrivateApi
{
    public class Service
    {
        public Models.ResponseModel Translate(string current_lang, string target_lang, string text)
        {
            var current_lang_short = current_lang.Split('-')[0];
            var target_lang_short = target_lang.Split('-')[0];

            string cookie = "NID=511=Hc2-gZD9_xR44x-ZJMAcPSHICPODQC1p4-Jqj7HQJbynWV1LDYaKpCD0rLK7SnRPNK76DhXEQ9DXr3_SFrxDsrBCJwH0fVopMpV-brfyGKW2EDfhtBE6c08U3z4BEXNO0chb2J8QE8IRp7PfG9PpEbJDGxE5nNXf0k7lkGqDwb4";
            string xClientData = "UlJDQUVTT1FEeWkwaDhZcWhieUYydWZfVHJqLVNUVlMtajc5QjZSTFB1R3VkR0MyZnBZMEFRT255TS01Q0poWTBoOHZhYWVyYnV5SkVfMUppUzJB";
            string path = $"/translate_a/single?dj=1&sl={current_lang_short}&tl={target_lang_short}&hl={current_lang}&ie=UTF-8&oe=UTF-8&client=at&dt=t&dt=ld&dt=qca&dt=rm&dt=bd&dt=md&dt=ss&dt=ex&dt=gt&dt=sos";

            var client = new RestClient($"https://translate.google.com{path}");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("scheme", "https");
            request.AddHeader("method", "POST");
            request.AddHeader("path", path);
            request.AddHeader("authority", "translate.google.com");
            request.AddHeader("cookie", cookie);
            client.UserAgent = "GoogleTranslate/6.27.0.08.415126308 (Linux; U; Android 7.1.2; SM-G973N)";
            request.AddHeader("x-client-data", xClientData);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("accept-encoding", "gzip, deflate");
            request.AddParameter("POST", $"{path} HTTP/2");
            request.AddParameter("Host", "translate.google.com");
            request.AddParameter("Cookie", cookie);
            request.AddParameter("User-Agent", "GoogleTranslate/6.27.0.08.415126308 (Linux; U; Android 7.1.2; SM-G973N)");
            request.AddParameter("X-Client-Data", xClientData);
            request.AddParameter("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("Accept-Encoding", "gzip, deflate");
            request.AddParameter("q", $"{text}");
            IRestResponse response = client.Execute(request);

            var response_api = JsonConvert.DeserializeObject<Models.ResponseModel>(response.Content);

            return response_api;
        }

        public string TranslateHtml(string html, string current_lang, string target_lang)
        {
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);
            TranslateHtmlNodeCollection(htmlDocument.DocumentNode.ChildNodes, current_lang: current_lang, target_lang: target_lang);
            return htmlDocument.DocumentNode.OuterHtml;
        }

        private void TranslateHtmlNodeCollection(HtmlAgilityPack.HtmlNodeCollection nodeCollection, string current_lang, string target_lang)
        {
            if (nodeCollection == null || nodeCollection.Count == 0)
                return;

            foreach (var node in nodeCollection)
            {
                if (node.Name == "#text" && node.InnerText != "\n")
                    node.InnerHtml = Translate(current_lang.ToLower(), target_lang, node.InnerText).sentences[0].trans;

                TranslateHtmlNodeCollection(node.ChildNodes, current_lang, target_lang);
            }
        }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SomeBlog.Integration.Wikipedia
{
    public class Service
    {
        public string GetContent(string title, string langCode = "tr")
        {
            WebClient client = new WebClient();

            using (Stream stream = client.OpenRead($"http://{langCode}.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&explaintext=1&titles={title}"))
            using (StreamReader reader = new StreamReader(stream))
            {
                JsonSerializer ser = new JsonSerializer();
                Result result = ser.Deserialize<Result>(new JsonTextReader(reader));
                if (result == null || result.query == null || result.query.pages.Count == 0)
                    return "no-content";

                return result.query.pages.Values.FirstOrDefault().extract;
            }
        }
    }

    public class Result
    {
        public Query query { get; set; }
    }

    public class Query
    {
        public Dictionary<string, Page> pages { get; set; }
    }

    public class Page
    {
        public string extract { get; set; }
    }
}

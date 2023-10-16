using Newtonsoft.Json;
using SomeBlog.Integration.Unsplash.Model;
using System.Net;

namespace SomeBlog.Integration.Unsplash
{
    public class Gatherer
    {
        public Model.SearchApiResult Search(string query, int page = 1)
        {
            var url = $"https://unsplash.com/napi/search/photos?query={query}&per_page=20&page={page}";
            var json = "";
            try
            {
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString(url);
                };
            }
            catch
            {
                return new SearchApiResult()
                {
                    Results = null,
                    TotalCount = 0,
                    TotalPages = 0
                };
            }

            var data = JsonConvert.DeserializeObject<SearchApiResult>(json);

            return data;
        }
    }
}

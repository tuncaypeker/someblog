namespace SomeBlog.Integration.Google.GoogleTrends
{
    using SomeBlog.Integration.Google.GoogleTrends.Dto;
    using System.IO;
    using System.Xml.Serialization;

    public class Service
    {
        public TrendingSearchRssDto GetTrendingSearches(string geo = "TR")
        {
            var path = $"https://trends.google.com/trends/trendingsearches/daily/rss?geo={geo}";

            var client = new RestSharp.RestClient(path);
            client.Timeout = -1;
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36";

            var request = new RestSharp.RestRequest(RestSharp.Method.GET);
            request.AddHeader("accept", "application/json, text/plain, */*");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("accept-language", "en-US,en;q=0.9,tr;q=0.8,fi;q=0.7");

            var response = client.Execute(request);
            
            XmlSerializer serializer = new XmlSerializer(typeof(TrendingSearchRssDto));
            using (StringReader reader = new StringReader(response.Content))
            {
               var test = (TrendingSearchRssDto)serializer.Deserialize(reader);

                return test;
            }
        }
    }
}

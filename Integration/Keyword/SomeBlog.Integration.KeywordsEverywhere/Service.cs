using Newtonsoft.Json;
using RestSharp;

namespace SomeBlog.Integration.KeywordsEveryWhere
{
    public class Service
    {
        public KeywordsApiModel Request(string keyword)
        {
            var client = new RestClient("https://api.keywordseverywhere.com/v1/get_keyword_data");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer 1a8271f888a1e149166e");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("dataSource", "gkp");
            request.AddParameter("country", "us");
            request.AddParameter("currency", "USD");
            request.AddParameter("kw[]", keyword);
            IRestResponse response = client.Execute(request);
            
            var keywordsApiModel = JsonConvert.DeserializeObject<KeywordsApiModel>(response.Content);

            return keywordsApiModel;
        }
    }
}

using Newtonsoft.Json;
using RestSharp;

namespace SomeBlog.Integration.Bing.Webmaster
{
    /// <summary>
    /// https://www.bing.com/indexnow#generateApiKey
    /// </summary>
    public class IndexNow
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyLocation"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool Submit(string url, string key, string keyLocation)
        {
            var path = $"https://bing.com/indexnow?url={url}&key={key}";

            var restClient = new RestClient(path);
            restClient.Timeout = -1;

            var restRequest = new RestRequest(Method.GET);

            var restResponse = restClient.Execute(restRequest);

            return restResponse.IsSuccessful;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">elinislesin.com</param>
        /// <param name="key"></param>
        /// <param name="keyLocation"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool SubmitBulk(string host, string key, string keyLocation, string[] urlList)
        {
            var path = $"https://bing.com/indexnow";

            var restClient = new RestClient(path);
            restClient.Timeout = -1;

            var restRequest = new RestRequest(Method.POST);
            var indexNowDto = new Dto.IndexNowDto()
            {
                host = host,
                key = key,
                //keyLocation = keyLocation,
                urlList = urlList
            };

            restRequest.AddJsonBody(JsonConvert.SerializeObject(indexNowDto));
            restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");

            var restRespone = restClient.Execute(restRequest);

            return restRespone.IsSuccessful;
        }
    }
}

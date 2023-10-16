using Newtonsoft.Json;
using RestSharp;
using System;

namespace SomeBlog.Integration.Whois.JsonWhois
{
    public class Service
    {
        /// <summary>
        /// https://jsonwhois.com/dashboard
        /// Temp mail ile kayit olup alabilirsin
        /// </summary>
        /// <param name="domain">hazirbilgi.net</param>
        /// <returns></returns>
        public Dto.WhoisDto GetWhois(string domain)
        {
            string apiKey = "578ae0d617e17c145c7d839de859999d";

            try
            {
                var client = new RestClient($"https://jsonwhois.com/api/v1/whois?domain={domain}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", "Token token=" + apiKey);
                IRestResponse response = client.Execute(request);

                Dto.WhoisDto result = JsonConvert.DeserializeObject<Dto.WhoisDto>(response.Content);

                return result;
            }
            catch (Exception e)
            {
                return new Dto.WhoisDto();
            }
        }
    }
}

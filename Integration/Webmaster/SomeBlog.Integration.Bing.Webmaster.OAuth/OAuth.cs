using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace SomeBlog.Integration.Bing.Webmaster.OAuth
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/bingwebmaster/oauth2?source=recommendations
    /// </summary>
    public class OAuth
    {
        private readonly string _clientName = "Someblog58";
        private readonly string _callBack = "https://someblog58.com/callback";
        private readonly string _clientId = "baaac51803694fe4a7a552ec85b6a212";
        private readonly string _clientSecret = "A9LWcQCztHENm1LC01qcr2WXtR8zS-h4zyT2-qyUGqNJGdxQV5YvLkVsEe5Z2bzCTjluwtcE4RJdu7rUPgHqNw";

        public string GenerateOAuthPath()
        {
            //https://www.bing.com/webmasters/oauth/authorize?response_type=code&client_id=baaac51803694fe4a7a552ec85b6a212&redirect_uri=https%3A%2F%2Fsomeblog58.com%2Fcallback&scope=webmaster.manage
            return $"https://www.bing.com/webmasters/oauth/authorize?response_type=code&client_id={_clientId}&redirect_uri={_callBack}&scope=webmaster.manage";
        }

        public void GetAcessToken(string _code)
        {
            try
            {
                HttpWebRequest req = WebRequest.CreateHttp("https://www.bing.com/webmasters/oauth/token");
                req.Method = WebRequestMethods.Http.Post;
                req.ContentType = "application/x-www-form-urlencoded";

                StringBuilder content = new StringBuilder();
                content.AppendFormat("code={0}&", Uri.EscapeDataString(_code));
                content.AppendFormat("client_id={0}&", Uri.EscapeDataString(_clientId));
                content.AppendFormat("client_secret={0}&", Uri.EscapeDataString(_clientSecret));
                content.AppendFormat("redirect_uri={0}&", Uri.EscapeDataString(_callBack));
                content.AppendFormat("grant_type={0}", Uri.EscapeDataString("authorization_code"));

                var data = Encoding.ASCII.GetBytes(content.ToString());

                using (var stream = req.GetRequestStreamAsync().Result)
                {
                    stream.WriteAsync(data, 0, data.Length).Wait();
                }

                string json;
                using (var res = req.GetResponseAsync().Result)
                {
                    using (var stream = res.GetResponseStream())
                    using (var sr = new StreamReader(stream))
                    {
                        json = sr.ReadToEndAsync().Result;
                    }
                }

                if (!string.IsNullOrWhiteSpace(json))
                {
                    //var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
                }
            }
            catch (WebException wex)
            {
                using (var stream = wex.Response.GetResponseStream())
                using (var sr = new StreamReader(stream))
                {
                    var t = sr.ReadToEndAsync().Result;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}

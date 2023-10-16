using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Indexing.v3;
using Google.Apis.Indexing.v3.Data;
using System;

namespace SomeBlog.Integration.Google.SearchConsole
{
    /// <summary>
    /// https://developers.google.com/webmaster-tools/search-console-api-original/v3?apix=true
    /// https://developers.google.com/webmaster-tools/search-console-api-original/v3/how-tos/search_analytics
    /// http://saka.docsio.net/67674095/unable-to-add-service-account-to-a-site-added-in-google-search-console-via-an-ap
    /// 
    /// Index yapabilmek icin, Site owner olman lazım. Bunu da https://www.google.com/webmasters/verification/details?siteUrl=https%3A%2F%2Fwww.hazirbilgi.net%2F&hl=tr Mülk sahiplerini yönet ekranından
    /// Service Account email'i ekleyerek yapiyorsun
    /// </summary>
    public class IndexWrapper
    {
        private readonly string credentialsJson;

        public IndexWrapper(string credentialsJson)
        {
            this.credentialsJson = credentialsJson;
        }

        private readonly string[] _scopes = {
            IndexingService.Scope.Indexing
        };

        public IndexingService GetIndexingService()
        {
            //using var stream = new FileStream("hazirbilgi-searchconsole-df920d80ebfc.json", FileMode.Open, FileAccess.Read);
            //var credential = GoogleCredential.FromStream(stream)

            var credential = GoogleCredential.FromJson(credentialsJson)
                .CreateScoped(_scopes)
                .UnderlyingCredential as ServiceAccountCredential;

            return new IndexingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
            });
        }

        /// <summary>
        /// URL_DELETED
        /// URL_UPDATED
        /// </summary>
        public bool Publish(string url)
        {
            try
            {
                var service = GetIndexingService();

                var urlNotification = new UrlNotification()
                {
                    Type = "URL_UPDATED",
                    Url = url,
                };

                var requestRespone = service.UrlNotifications.Publish(urlNotification);
                var response = requestRespone.Execute();

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
    }
}

using Google.Apis.Auth.OAuth2;
using Google.Apis.SearchConsole.v1;
using Google.Apis.Services;

namespace SomeBlog.Integration.Google.SearchConsole
{
    /// <summary>
    /// https://developers.google.com/webmaster-tools/search-console-api-original/v3?apix=true
    /// https://developers.google.com/webmaster-tools/search-console-api-original/v3/how-tos/search_analytics
    /// http://saka.docsio.net/67674095/unable-to-add-service-account-to-a-site-added-in-google-search-console-via-an-ap
    /// </summary>
    public class ServiceWrapper
    {
        private readonly string credentialsJson;

        public ServiceWrapper(string credentialsJson)
        {
            this.credentialsJson = credentialsJson;
        }

        private readonly string[] _scopes = {
            SearchConsoleService.Scope.WebmastersReadonly,
            SearchConsoleService.Scope.Webmasters,
        };

        public SearchConsoleService GetWebmastersService()
        {
            var credential = GoogleCredential.FromJson(credentialsJson)
                .CreateScoped(_scopes)
                .UnderlyingCredential as ServiceAccountCredential;

            return new SearchConsoleService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
            });
        }
    }
}

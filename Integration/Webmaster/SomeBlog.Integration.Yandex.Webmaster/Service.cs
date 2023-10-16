using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SomeBlog.Integration.Yandex.Webmaster.Dto;
using System;
using System.Collections.Generic;

namespace SomeBlog.Integration.Yandex.Webmaster
{
    /// <summary>
    /// https://yandex.com/dev/webmaster/doc/dg/concepts/getting-started.html
    /// </summary>
    public class Service
    {
        private readonly string BaseApiPath = "https://api.webmaster.yandex.net";
        private string AccessKey;
        private string userId;

        public Service(string accessKey)
        {
            AccessKey = accessKey;
            userId = GetUserId();
        }

        /// <summary>
        /// http://www.sinanbozkus.com/yandex-metrica-api-kullanimi/
        /// 
        /// buraya bakarak nasıl acces token alındıgına baktık
        /// Getting the user ID
        /// </summary>
        public string GetUserId()
        {
            var client = buildClient($"{BaseApiPath}/v4/user");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
            var json = JObject.Parse(response.Content);
            if (json["error_message"] != null)
                return "";

            return json["user_id"].ToString();
        }

        /// <summary>
        /// https://yandex.com/dev/webmaster/doc/dg/reference/hosts.html
        /// </summary>
        public ResponseDto<GetSitesResponseDto> GetSites()
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
            var getSitesResponseDto = JsonConvert.DeserializeObject<GetSitesResponseDto>(response.Content);

            return new ResponseDto<GetSitesResponseDto>()
            {
                Data = getSitesResponseDto,
                IsSucceded = true,
                Message = ""
            };
        }

        /// <summary>
        /// https://yandex.com/dev/webmaster/doc/dg/reference/hosts-add-site.html
        /// </summary>
        /// <param name="domain"></param>
        public void AddSite(string domain)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts");
            var request = buildRequest(Method.POST, new Dictionary<string, string>() {
                {"Content-Type","application/json" }
            });

            request.AddJsonBody("{\"host_url\": \"" + domain + "\"}");

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// https://yandex.com/dev/webmaster/doc/dg/reference/hosts-id.html
        /// </summary>
        /// <param name="hostId"></param>
        public ResponseDto<HostDto> GetSite(string hostId)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
            var hostDto = JsonConvert.DeserializeObject<HostDto>(response.Content);

            return new ResponseDto<HostDto>()
            {
                Data = hostDto,
                IsSucceded = false,
                Message = ""
            };
        }

        /// <summary>
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-id-summary.html
        /// Returns general information about site indexing.
        /// {
        ///   "sqi": 1,
        ///   "excluded_pages_count": 1,
        ///   "searchable_pages_count": 1,
        ///   "site_problems": {
        ///     "FATAL": 1
        ///   }
        ///}
        /// </summary>
        /// <param name="hostId"></param>
        public ResponseDto<GetSiteStatisticsResponseDto> GetSiteStatistics(string hostId)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/summary/");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
            var getSiteStatisticsResponseDto = JsonConvert.DeserializeObject<GetSiteStatisticsResponseDto>(response.Content);

            return new ResponseDto<GetSiteStatisticsResponseDto>()
            {
                Data = getSiteStatisticsResponseDto,
                IsSucceded = false,
                Message = ""
            };
        }

        /// <summary>
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-id-important-urls.html
        /// Returns indexing information on the site pages you selected in Yandex.Webmaster (Indexing → Monitoring important pages). For more information, see this Help section.
        /// https://yandex.com/support/webmaster/service/tracking-url.html
        /// </summary>
        /// <param name="hostId"></param>
        public ResponseDto<GetImportantUrlsResponseDto> GetImportantUrls(string hostId)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/important-urls/");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
            var getImportantUrlsResponseDto = JsonConvert.DeserializeObject<GetImportantUrlsResponseDto>(response.Content);

            return new ResponseDto<GetImportantUrlsResponseDto>()
            {
                Data = getImportantUrlsResponseDto,
                IsSucceded = false,
                Message = ""
            };
        }

        /// <summary>
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-id-important-urls-history.html#host-id-important-urls-history
        /// Returns information about changes to the specified page (selected in Yandex.Webmaster on the Indexing → Monitoring important pages page). For more information, see this Help section.
        /// https://yandex.com/support/webmaster/service/tracking-url.html
        /// </summary>
        /// <param name="hostId"></param>
        public ResponseDto<GetImportantUrlHistoryResponseDto> GetImportantUrlHistory(string hostId, string url)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/important-urls/history?url={url}");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
            var responseDto = JsonConvert.DeserializeObject<GetImportantUrlHistoryResponseDto>(response.Content);

            return new ResponseDto<GetImportantUrlHistoryResponseDto>()
            {
                Data = responseDto,
                IsSucceded = false,
                Message = ""
            };
        }

        /// <summary>
        /// Returns a list of Sitemap files detected by Yandex robots.
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-sitemaps-get.html
        /// </summary>
        /// <param name="hostId"></param>
        public ResponseDto<GetSitemapsByYandexResponseDto> GetSitemapsByYandex(string hostId, string parentId, int limit = 10, string fromSitemapId = "")
        {
            if (string.IsNullOrEmpty(hostId))
                return new ResponseDto<GetSitemapsByYandexResponseDto>()
                {
                    Data = null,
                    IsSucceded = false,
                    Message = "Host Id bilgisi gelmedi"
                };

            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/sitemaps?parent_id={parentId}&limit={limit}&from={fromSitemapId}");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            var responseDto = JsonConvert.DeserializeObject<GetSitemapsByYandexResponseDto>(response.Content);

            if (responseDto.error_code != null) {
                return new ResponseDto<GetSitemapsByYandexResponseDto>()
                {
                    IsSucceded = false,
                    Message = "",
                    Data = new GetSitemapsByYandexResponseDto()
                    {
                        error_code = responseDto.error_code,
                        sitemaps = new List<SitemapByYandex>()
                    }
                };
            }

            return new ResponseDto<GetSitemapsByYandexResponseDto>()
            {
                Data = responseDto,
                IsSucceded = true,
                Message = ""
            };
        }

        /// <summary>
        /// Returns detailed information about the Sitemap file, including the file type, date and method used for 
        /// uploading the file to Yandex.Webmaster, the date the service processed the file, the number of URLs in the file and the number and type of errors found.
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-sitemaps-sitemap-id-get.html
        /// </summary>
        /// <param name="hostId"></param>
        public void GetSitemap(string hostId, string sitemapId)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/sitemaps?sitemap_id={sitemapId}");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);

        }

        /// <summary>
        /// Returns a list of Sitemap files detected by Yandex robots.
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-user-added-sitemaps-get.html
        /// </summary>
        /// <param name="hostId"></param>
        public ResponseDto<GetSitemapsByUserResponseDto> GetSitemapsByUser(string hostId, string parentId, int limit = 100, string fromSitemapId = "")
        {
            if (string.IsNullOrEmpty(hostId))
                return new ResponseDto<GetSitemapsByUserResponseDto>()
                {
                    Data = null,
                    IsSucceded = false,
                    Message = "Host Id bilgisi gelmedi"
                };

            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/user-added-sitemaps?offset={fromSitemapId}&limit={limit}");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
            var responseDto = JsonConvert.DeserializeObject<GetSitemapsByUserResponseDto>(response.Content);

            if (responseDto.error_code != null)
            {
                return new ResponseDto<GetSitemapsByUserResponseDto>()
                {
                    IsSucceded = false,
                    Message = "",
                    Data = new GetSitemapsByUserResponseDto()
                    {
                        error_code = responseDto.error_code,
                        sitemaps = new List<SitemapByUser>()
                    }
                };
            }

            return new ResponseDto<GetSitemapsByUserResponseDto>()
            {
                Data = responseDto,
                IsSucceded = true,
                Message = ""
            };
        }

        /// <summary>
        /// Returns a list of Sitemap files detected by Yandex robots.
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-user-added-sitemaps-get.html
        /// </summary>
        /// <param name="hostId"></param>
        public bool AddSitemap(string hostId, string sitemapUrl)
        {
            try
            {
                var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/user-added-sitemaps");
                var request = buildRequest(Method.POST, new Dictionary<string, string>() {
                    {"Content-Type","application/json" }
                });

                request.AddJsonBody("{\"url\": \"" + sitemapUrl + "\"}");

                IRestResponse response = client.Execute(request);

                var addSitemapResponseDto = JsonConvert.DeserializeObject<AddSitemapResponseDto>(response.Content);

                return addSitemapResponseDto.error_code == null;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        /// <summary>
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-user-added-sitemaps-sitemap-id-get.html
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="sitemapId"></param>
        public ResponseDto<SitemapByUser> GetSitemapByUser(string hostId, string sitemapId)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/user-added-sitemaps/{sitemapId}");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
            var responseDto = JsonConvert.DeserializeObject<SitemapByUser>(response.Content);


            return new ResponseDto<SitemapByUser>()
            {
                Data = responseDto,
                IsSucceded = false,
                Message = ""
            };
        }

        /// <summary>
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-user-added-sitemaps-sitemap-id-delete.html
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="sitemapId"></param>
        public void RemoveSitemapByUser(string hostId, string sitemapId)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/user-added-sitemaps/{sitemapId}");
            var request = buildRequest(Method.DELETE);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Gets a list of the TOP-3000 search queries the site was shown for in the search results over the past week. You can choose 500 queries with the most displays or with the most clicks.
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-search-queries-popular.html
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="orderBy">TOTAL_SHOWS, TOTAL_CLICKS</param>
        /// <param name="queryIndicator">TOTAL_SHOWS, TOTAL_CLICKS, AVG_SHOW_POSITION, AVG_CLICK_POSITION</param>
        /// <param name="deviceTypeIndicator">ALL, DESKTOP,MOBILE_AND_TABLET,MOBILE,TABLET</param>s
        public void GetSearchQueriesPopular(string hostId,
            DateTime date_from,
            DateTime date_to,
            string orderBy = "TOTAL_SHOWS",
            string queryIndicator = "AVG_SHOW_POSITION",
            string deviceTypeIndicator = "MOBILE",
            int offset = 0,
            int limit = 500
            )
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/search-queries/popular?" +
                $"order_by={orderBy}&" +
                $"query_indicator={queryIndicator}&" +
                $"device_type_indicator={deviceTypeIndicator}&" +
                $"date_from={date_from.ToString("yyyy-MM-dd")}&" +
                $"date_to={date_to.ToString("yyyy-MM-dd")}&" +
                $"offset={offset}&" +
                $"limit={limit}");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Allows you to get the history of changes in indicators for all search queries for a certain period of time.
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-search-queries-history-all.html#host-search-queries-history-all
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="queryIndicator">TOTAL_SHOWS, TOTAL_CLICKS, AVG_SHOW_POSITION, AVG_CLICK_POSITION</param>
        /// <param name="deviceTypeIndicator">ALL, DESKTOP,MOBILE_AND_TABLET,MOBILE,TABLET</param>s
        public void GetSearchQueriesAll(string hostId,
            DateTime date_from,
            DateTime date_to,
            string queryIndicator = "AVG_SHOW_POSITION",
            string deviceTypeIndicator = "ALL"
            )
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/search-queries/all/history?" +
                $"query_indicator={queryIndicator}&" +
                $"device_type_indicator={deviceTypeIndicator}&" +
                $"date_from={date_from.ToString("yyyy-MM-dd")}&" +
                $"date_to={date_to.ToString("yyyy-MM-dd")}");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Allows you to get the history of changes in indicators for a search query for a period of time.
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-search-queries-history.html#host-search-queries-history
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="queryIndicator">TOTAL_SHOWS, TOTAL_CLICKS, AVG_SHOW_POSITION, AVG_CLICK_POSITION</param>
        /// <param name="deviceTypeIndicator">ALL, DESKTOP,MOBILE_AND_TABLET,MOBILE,TABLET</param>s
        public void GetSearchQuery(string hostId,
            string queryId,
            DateTime date_from,
            DateTime date_to,
            string queryIndicator = "TOTAL_SHOWS",
            string deviceTypeIndicator = "MOBILE"
            )
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/search-queries/{queryId}/history?" +
                $"query_indicator={queryIndicator}&" +
                $"device_type_indicator={deviceTypeIndicator}&" +
                $"date_from={date_from.ToString("yyyy-MM-dd")}&" +
                $"date_to={date_to.ToString("yyyy-MM-dd")}");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Getting a list of tasks for reindexing
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-recrawl-get.html#host-recrawl-get
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="date_from"></param>
        /// <param name="date_to"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        public void GetRecrawlQueue(string hostId,
            DateTime date_from,
            DateTime date_to,
            int offset = 0,
            int limit = 50
            )
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/recrawl/queue?" +
                $"offset={offset}&" +
                $"limit={limit}&" +
                $"date_from={date_from.ToString("yyyy-MM-dd")}&" +
                $"date_to={date_to.ToString("yyyy-MM-dd")}"
                );
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Sending a page for reindexing
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-recrawl-post.html#host-recrawl-post
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="url"></param>
        public ResponseDto<PostRecrawlQueueDto> PostRecrawlQueue(string hostId, string url)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/recrawl/queue");
            var request = buildRequest(Method.POST, new Dictionary<string, string>() {
                {"Content-Type","application/json" }
            });

            request.AddJsonBody("{\"url\": \"" + url + "\"}");

            var response = client.Execute(request);
            var responseDto = JsonConvert.DeserializeObject<PostRecrawlQueueDto>(response.Content);

            if (responseDto.error_code != null)
            {
                return new ResponseDto<PostRecrawlQueueDto>()
                {
                    IsSucceded = false,
                    Message = "",
                    Data = new PostRecrawlQueueDto()
                    {
                        error_code = responseDto.error_code,
                        QuotaReminder = 0,
                        TaskId = ""
                    }
                };
            }

            return new ResponseDto<PostRecrawlQueueDto>()
            {
                Data = responseDto,
                IsSucceded = true,
                Message = ""
            };

        }

        /// <summary>
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-recrawl-quota-get.html#host-recrawl-quota-get
        /// Checking the reindexing quota
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="sitemapId"></param>
        public ResponseDto<GetRecrawlQuotaResponseDto> GetRecralwQuota(string hostId)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/recrawl/quota");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
            var responseDto = JsonConvert.DeserializeObject<GetRecrawlQuotaResponseDto>(response.Content);

            return new ResponseDto<GetRecrawlQuotaResponseDto>()
            {
                Data = responseDto,
                IsSucceded = false,
                Message = ""
            };
        }

        /// <summary>
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-recrawl-task-get.html#host-recrawl-task-get
        /// Checking the reindexing task status
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="sitemapId"></param>
        public void GetRecrawlStatus(string hostId, string taskId)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/recrawl/queue/{taskId}");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-diagnostics-get.html#host-diagnostics-get
        /// Returns information about errors on the site. If the site has Turbo pages, the response may contain information about their diagnostics (for more information, see the documentation).
        /// https://tech.yandex.com/turbo/doc/api/ref/get-diagnostics-docpage/
        /// </summary>
        public void GetSiteDiagnostics(string hostId)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/diagnostics");
            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Getting the site indexing history
        /// Returns the quantity of indexed site pages, as well as their HTTP status for a specific period. By default, data for the current day is returned.
        /// https://yandex.com/dev/webmaster/doc/dg/reference/hosts-indexing-history.html#hosts-indexing-history
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="date_from"></param>
        /// <param name="date_to"></param>
        public void GetSiteIndexHistory(string hostId,
            DateTime date_from,
            DateTime date_to)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/indexing/history?" +
                $"date_from={date_from}&" +
                $"date_to={date_to}");

            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Viewing examples of downloaded pages
        /// Returns the list of indexed site pages (maximum 50,000 URLs).
        /// https://yandex.com/dev/webmaster/doc/dg/reference/hosts-indexing-samples.html#hosts-indexing-samples
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        public void GetSiteIndexedPages(string hostId,
            int offset,
            int limit)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/indexing/samples?" +
                $"offset={offset}&" +
                $"limit={limit}");

            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Returns the number of pages in search results for a specific time period. By default, data for the current day is returned.
        /// https://yandex.com/dev/webmaster/doc/dg/reference/hosts-indexing-insearch-history.html#hosts-indexing-insearch-history
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="date_from"></param>
        /// <param name="date_to"></param>
        public void GetNumberOfPagesInSearch(string hostId,
           DateTime date_from,
           DateTime date_to)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/search-urls/in-search/history?" +
                $"date_from={date_from}&" +
                $"date_to={date_to}");

            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Getting the history of pages added to and removed from search results
        /// Returns the number of pages that appeared in search and were removed from search results in a given period. By default, data for the current day is returned.
        /// https://yandex.com/dev/webmaster/doc/dg/reference/hosts-search-events-history.html#hosts-search-events-history
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="date_from"></param>
        /// <param name="date_to"></param>
        public void GetHistoryOfPagesInSearch(string hostId,
          DateTime date_from,
          DateTime date_to)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/search-urls/events/history?" +
                $"date_from={date_from}&" +
                $"date_to={date_to}");

            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Getting information about broken internal links on the site
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-links-internal-samples.html#host-links-internal-samples
        /// Returns examples of broken internal links on the site. The list of link examples is based on the data from the Yandex robot. The link is considered broken if there is an error in the URL of the page, the URL of the page changed or the page doesn't exist.
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        public void GetCrawledErrorPages(string hostId,
            int offset = 0,
            int limit = 10)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/links/internal/broken/samples?" +
                $"offset={offset}&" +
                $"limit={limit}");

            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Getting information about external links to the site
        /// https://yandex.com/dev/webmaster/doc/dg/reference/host-links-external-samples.html#host-links-external-sampless
        /// Returns examples of external links to site pages.
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        public void GetExternalLinks(string hostId,
            int offset = 0,
            int limit = 10)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/links/external/samples?" +
                $"offset={offset}&" +
                $"limit={limit}");

            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// Getting information about external links to the site
        /// Getting the history of changes in the number of external links to the site
        /// Returns the history of changes in the number of external links to the site.
        /// </summary>
        /// <param name="hostId"></param>
        public void GetExternalLinksNumberHistory(string hostId)
        {
            var client = buildClient($"{BaseApiPath}/v4/user/{userId}/hosts/{hostId}/links/external/history?" +
                $"indicator=LINKS_TOTAL_COUNT");

            var request = buildRequest(Method.GET);

            IRestResponse response = client.Execute(request);
        }







        private RestClient buildClient(string path)
        {
            var client = new RestClient(path);
            client.Timeout = -1;

            return client;
        }

        private RestRequest buildRequest(RestSharp.Method method, Dictionary<string, string> additionalHeaders = null)
        {
            var request = new RestRequest(method);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "OAuth " + AccessKey);

            if (additionalHeaders != null)
            {
                foreach (var keyValue in additionalHeaders)
                {
                    request.AddHeader(keyValue.Key, keyValue.Value);
                }
            }

            return request;
        }


    }
}


using BingWebmasterService;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using SomeBlog.Integration.Bing.Webmaster.Dto;

namespace SomeBlog.Integration.Bing.Webmaster
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.bing.webmaster.api.interfaces.iwebmasterapi?view=bing-webmaster-dotnet
    /// </summary>
    public class Service
    {
        WebmasterApiClient api;
        public Service(string apiKey)
        {
            api = new WebmasterApiClient(WebmasterApiClient.EndpointConfiguration.BasicHttpBinding_IWebmasterApi, "https://www.bing.com/webmasterapi/api.svc/soap?apikey=" + apiKey);
        }

        public Dto.Response<Dto.UrlInfoDto> GetUrlInfo(string domain, string page)
        {
            var response = new Dto.Response<Dto.UrlInfoDto>(false, "");

            try
            {
                var data = api.GetUrlInfoAsync(domain, page).Result;

                response.IsSucceed = true;
                response.Data = Dto.UrlInfoDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<LinkDetailsDto> GetUrlLinks(string domain, string url, short page = 1)
        {
            var response = new Dto.Response<Dto.LinkDetailsDto>(false, "");

            try
            {
                var data = api.GetUrlLinksAsync(domain, url, page).Result;

                response.IsSucceed = true;
                response.Data = Dto.LinkDetailsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }

        //FETCH URL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        public bool FetchUrl(string domain, string url)
        {
            try
            {
                api.FetchUrlAsync(domain, url).Wait();
                return true;
            }
            catch (FaultException<ApiFault> fault)
            {
                return false;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
        public Dto.Response<Dto.FetchedUrlDetailsDto> GetFetchedUrlDetails(string domain, string url)
        {
            var response = new Dto.Response<Dto.FetchedUrlDetailsDto>(false, "");

            try
            {
                var data = api.GetFetchedUrlDetailsAsync(domain, url).Result;

                response.IsSucceed = true;
                response.Data = Dto.FetchedUrlDetailsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Response<List<Dto.FetchedUrlDto>> GetFetchedUrls(string domain)
        {
            var response = new Dto.Response<List<Dto.FetchedUrlDto>>(false, "");

            try
            {
                var data = api.GetFetchedUrlsAsync(domain).Result;

                response.IsSucceed = true;
                response.Data = Dto.FetchedUrlDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }


        //TRAFIC STATS
        public Dto.Response<List<Dto.RankAndTrafficStatsDto>> GetQueryTrafficStats(string domain, string query)
        {
            var response = new Dto.Response<List<Dto.RankAndTrafficStatsDto>>(false, "");

            try
            {
                var data = api.GetQueryTrafficStatsAsync(domain, query).Result;

                response.IsSucceed = true;
                response.Data = Dto.RankAndTrafficStatsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<List<Dto.RankAndTrafficStatsDto>> GetRankAndTrafficStats(string domain)
        {
            var response = new Dto.Response<List<Dto.RankAndTrafficStatsDto>>(false, "");

            try
            {
                var data = api.GetRankAndTrafficStatsAsync(domain).Result;

                response.IsSucceed = true;
                response.Data = Dto.RankAndTrafficStatsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<Dto.UrlTrafficInfoDto> GetUrlTrafficInfo(string domain, string url)
        {
            var response = new Dto.Response<Dto.UrlTrafficInfoDto>(false, "");

            try
            {
                var data = api.GetUrlTrafficInfoAsync(domain, url).Result;

                response.IsSucceed = true;
                response.Data = Dto.UrlTrafficInfoDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }

        //QUERY STATS
        public Dto.Response<List<Dto.QueryStatsDto>> GetQueryStats(string domain)
        {
            var response = new Dto.Response<List<Dto.QueryStatsDto>>(false, "");

            try
            {
                var data = api.GetQueryStatsAsync(domain).Result;

                response.IsSucceed = true;
                response.Data = Dto.QueryStatsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<List<Dto.QueryStatsDto>> GetPageStats(string page)
        {
            var response = new Dto.Response<List<Dto.QueryStatsDto>>(false, "");

            try
            {
                var data = api.GetPageStatsAsync(page).Result;

                response.IsSucceed = true;
                response.Data = Dto.QueryStatsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<List<Dto.QueryStatsDto>> GetPageQueryStats(string domain, string page)
        {
            var response = new Dto.Response<List<Dto.QueryStatsDto>>(false, "");

            try
            {
                var data = api.GetPageQueryStatsAsync(domain, page).Result;

                response.IsSucceed = true;
                response.Data = Dto.QueryStatsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<List<Dto.QueryStatsDto>> GetQueryPageStats(string domain, string query)
        {
            var response = new Dto.Response<List<Dto.QueryStatsDto>>(false, "");

            try
            {
                var data = api.GetQueryPageStatsAsync(domain, query).Result;

                response.IsSucceed = true;
                response.Data = Dto.QueryStatsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }

        //SITES
        public Dto.Response<List<Dto.SiteDto>> GetUserSites()
        {
            var response = new Dto.Response<List<Dto.SiteDto>>(false, "");

            try
            {
                var data = api.GetUserSitesAsync().Result;

                response.IsSucceed = true;
                response.Data = Dto.SiteDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<bool> AddSite(string domain)
        {
            try
            {
                api.AddSiteAsync(domain).Wait();
                return new Response<bool>()
                {
                    Data = true,
                    IsSucceed = true,
                    Message = ""
                };
            }
            catch (FaultException<ApiFault> fault)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = fault.Message
                };
            }
            catch (Exception exc)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = exc.Message
                };
            }
        }
        public Dto.Response<bool> RemoveSite(string domain)
        {
            try
            {
                api.RemoveSiteAsync(domain).Wait();
                return new Response<bool>()
                {
                    Data = true,
                    IsSucceed = true,
                    Message = ""
                };
            }
            catch (FaultException<ApiFault> fault)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = fault.Message
                };
            }
            catch (Exception exc)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = exc.Message
                };
            }
        }
        public Dto.Response<bool> CheckVerification(string domain)
        {
            try
            {
                var result = api.CheckSiteVerificationAsync(domain).Result;
                return new Response<bool>()
                {
                    Data = result,
                    IsSucceed = true,
                    Message = ""
                };
            }
            catch (FaultException<ApiFault> fault)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = fault.Message
                };
            }
            catch (Exception exc)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = exc.Message
                };
            }
        }


        //SITEMAPS
        public Dto.Response<bool> RemoveFeed(string domain, string feedUrl)
        {
            try
            {
                api.RemoveFeedAsync(domain, feedUrl).Wait();
                return new Response<bool>()
                {
                    Data = true,
                    IsSucceed = true,
                    Message = ""
                };
            }
            catch (FaultException<ApiFault> fault)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = fault.Message
                };
            }
            catch (Exception exc)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = exc.Message
                };
            }
        }
        public Dto.Response<bool> SubmitFeed(string domain, string feedUrl)
        {
            try
            {
                api.SubmitFeedAsync(domain, feedUrl).Wait();
                return new Response<bool>()
                {
                    Data = true,
                    IsSucceed = true,
                    Message = ""
                };
            }
            catch (FaultException<ApiFault> fault)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = fault.Message
                };
            }
            catch (Exception exc)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = exc.Message
                };
            }
        }
        public Dto.Response<List<FeedDto>> GetFeedDetails(string domain, string feedUrl)
        {
            var response = new Dto.Response<List<Dto.FeedDto>>(false, "");

            try
            {
                var data = api.GetFeedDetailsAsync(domain, feedUrl).Result;

                response.IsSucceed = true;
                response.Data = Dto.FeedDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<List<FeedDto>> GetFeeds(string domain)
        {
            var response = new Dto.Response<List<Dto.FeedDto>>(false, "");

            try
            {
                var data = api.GetFeedsAsync(domain).Result;

                response.IsSucceed = true;
                response.Data = Dto.FeedDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc) {
                response.Message = exc.Message;
            }

            return response;
        }

        //URL SUBMIT
        public Dto.Response<UrlSubmissionQuotaDto> GetUrlSubmissionQuota(string domain)
        {
            var response = new Dto.Response<Dto.UrlSubmissionQuotaDto>(false, "");

            try
            {
                var data = api.GetUrlSubmissionQuotaAsync(domain).Result;

                response.IsSucceed = true;
                response.Data = Dto.UrlSubmissionQuotaDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<bool> SubmitUrl(string domain, string url)
        {
            try
            {
                api.SubmitUrlAsync(domain, url).Wait();
                return new Response<bool>()
                {
                    Data = true,
                    IsSucceed = true,
                    Message = ""
                };
            }
            catch (FaultException<ApiFault> fault)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = fault.Message
                };
            }
            catch (Exception exc)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = exc.Message
                };
            }
        }
        public Dto.Response<bool> SubmitUrlBatch(string domain, List<string> urlList)
        {
            try
            {
                var quoata = api.GetUrlSubmissionQuotaAsync(domain).Result;
                if (quoata.DailyQuota == 0)
                    return new Response<bool>()
                    {
                        Data = false,
                        IsSucceed = false,
                        Message = "No quota for today"
                    }; ;

                api.SubmitUrlBatchAsync(domain, urlList).Wait();

                return new Response<bool>()
                {
                    Data = true,
                    IsSucceed = true,
                    Message = ""
                };
            }
            catch (FaultException<ApiFault> fault)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = fault.Message
                };
            }
            catch (Exception exc)
            {
                return new Response<bool>()
                {
                    Data = false,
                    IsSucceed = false,
                    Message = exc.Message
                };
            }
        }


        //CRAWL
        public Response<List<Dto.UrlWithCrawlIssuesDto>> GetCrawlIssues(string domain)
        {
            var response = new Dto.Response<List<Dto.UrlWithCrawlIssuesDto>>(false, "");

            try
            {
                var data = api.GetCrawlIssuesAsync(domain).Result;

                response.IsSucceed = true;
                response.Data = Dto.UrlWithCrawlIssuesDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<Dto.CrawlSettingsDto> GetCrawlSettings(string domain)
        {
            var response = new Dto.Response<Dto.CrawlSettingsDto>(false, "");

            try
            {
                var data = api.GetCrawlSettingsAsync(domain).Result;

                response.IsSucceed = true;
                response.Data = Dto.CrawlSettingsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<List<Dto.CrawlStatsDto>> GetCrawlStats(string domain)
        {
            var response = new Dto.Response<List<Dto.CrawlStatsDto>>(false, "");

            try
            {
                var data = api.GetCrawlStatsAsync(domain).Result;

                response.IsSucceed = true;
                response.Data = Dto.CrawlStatsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }


        //KEYWORDS
        public Dto.Response<Dto.KeywordDto> GetKeyword(string keyword, string country = "tr", string lang = "tr-TR")
        {
            var response = new Dto.Response<Dto.KeywordDto>(false, "");

            try
            {
                var data = api.GetKeywordAsync(keyword, country, lang, DateTime.Now.AddMonths(-3), DateTime.Now).Result;

                response.IsSucceed = true;
                response.Data = Dto.KeywordDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<List<Dto.KeywordStatsDto>> GetKeywordStats(string keyword, string country = "tr", string lang = "tr-TR")
        {
            var response = new Dto.Response<List<Dto.KeywordStatsDto>>(false, "");

            try
            {
                var data = api.GetKeywordStatsAsync(keyword, country, lang).Result;

                response.IsSucceed = true;
                response.Data = Dto.KeywordStatsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<List<Dto.DetailedQueryStatsDto>> GetQueryPageDetailStats(string domain, string query, string page)
        {
            var response = new Dto.Response<List<Dto.DetailedQueryStatsDto>>(false, "");

            try
            {
                if (string.IsNullOrEmpty(page))
                    page = domain;

                var data = api.GetQueryPageDetailStatsAsync(domain, query, page).Result;

                response.IsSucceed = true;
                response.Data = Dto.DetailedQueryStatsDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }
        public Dto.Response<List<Dto.KeywordDto>> GetRelatedKeywords(string query, string country = "tr", string lang = "tr-TR")
        {
            var response = new Dto.Response<List<Dto.KeywordDto>>(false, "");

            try
            {
                var data = api.GetRelatedKeywordsAsync(query, country, lang, DateTime.Now.AddMonths(-6), DateTime.Now).Result;

                response.IsSucceed = true;
                response.Data = Dto.KeywordDto.From(data);
            }
            catch (FaultException<ApiFault> fault)
            {
                response.Message = fault.Message;
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
            }

            return response;
        }

    }
}

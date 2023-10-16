using Newtonsoft.Json;
using RestSharp;
using System;

namespace SomeBlog.Integration.MozPro
{
    public class Service
    {
        //Keyword Research > Site Overview ekranından alabilirsin
        public readonly string accountId = "20017589"; //site rankings isteğinde request parameter içinde gidiyor
        public readonly string siteRankingsEndpoint = "https://moz.com/explorer/api/2.5/site/rankings";
        public readonly string siteRankingsCookie = "_gcl_au=1.1.1587853248.1631524319; _fbp=fb.1.1631524319197.41611897; hubspotutk=01b09236286543aedb83219e40df2c0e; __stripe_mid=f990c380-1fa2-4331-8c10-955f9df24b329d4a53; _gid=GA1.2.1859426265.1635922654; __hssrc=1; mozauth=xLaZi2zMbZcXCjkHJgrVDSnZdybRmV7reCLj86O86PiJ7XrDXxAvhTawgPwej71e; ajs_user_id=18007615; ajs_anonymous_id=%22857dd900-057a-4341-af09-5ae8c52802f8%22; ajs_group_id=20017589; __cf_bm=eWGKexCMyzsev2LvlmBcB1cL7Q3uVdpj8noNlC2rjR4-1635924546-0-AVJMZ/7TxaafULewC+gSapER0BIz6Dg9SEm5CXF/eZ3nsZBIl5N5iaFffpgow1s8FQIPHucR6Y4lz7xXnSvQnNi5EG6uozSP7BTuVVRkrGwebRR/yit29DwBNA31F/i14gk8RPU2mS4aNVfZAoppB6u6rkNNztUeUGkBID/iQywK; __hstc=103427807.01b09236286543aedb83219e40df2c0e.1631524321393.1635922654472.1635924547640.16; __stripe_sid=7fa06247-a79f-41d8-a326-e57b92e22230d01c78; _gat=1; _gat_UA-1870679-33=1; _uetsid=527dcb103c7311eca24fa7d10d9105df; _uetvid=a61f8110147211ecbc6c83ec0baa15b8; __hssc=103427807.6.1635924547640; _ga=GA1.2.950776625.1631524319; _ga_LGQZKGRBE5=GS1.1.1635924546.17.1.1635924698.31";


        //Keyword Research > Keyword Overview ekranından alabilrsin
        public readonly string analysisKeywordEndpoint = "https://moz.com/explorer/api/2.5/keyword/analysis";
        public readonly string analysisKeywordCookie = "_gcl_au=1.1.1587853248.1631524319; _fbp=fb.1.1631524319197.41611897; hubspotutk=01b09236286543aedb83219e40df2c0e; __stripe_mid=f990c380-1fa2-4331-8c10-955f9df24b329d4a53; _gid=GA1.2.1859426265.1635922654; __hssrc=1; mozauth=xLaZi2zMbZcXCjkHJgrVDSnZdybRmV7reCLj86O86PiJ7XrDXxAvhTawgPwej71e; ajs_user_id=18007615; ajs_anonymous_id=%22857dd900-057a-4341-af09-5ae8c52802f8%22; ajs_group_id=20017589; __cf_bm=eWGKexCMyzsev2LvlmBcB1cL7Q3uVdpj8noNlC2rjR4-1635924546-0-AVJMZ/7TxaafULewC+gSapER0BIz6Dg9SEm5CXF/eZ3nsZBIl5N5iaFffpgow1s8FQIPHucR6Y4lz7xXnSvQnNi5EG6uozSP7BTuVVRkrGwebRR/yit29DwBNA31F/i14gk8RPU2mS4aNVfZAoppB6u6rkNNztUeUGkBID/iQywK; __hstc=103427807.01b09236286543aedb83219e40df2c0e.1631524321393.1635922654472.1635924547640.16; __stripe_sid=7fa06247-a79f-41d8-a326-e57b92e22230d01c78; _uetsid=527dcb103c7311eca24fa7d10d9105df; _uetvid=a61f8110147211ecbc6c83ec0baa15b8; __hssc=103427807.6.1635924547640; _ga=GA1.2.950776625.1631524319; _ga_LGQZKGRBE5=GS1.1.1635924546.17.1.1635924698.31; _gat=1";

        //Link Research > Inbound Lİnks ekranından alabilirsin
        public readonly string analysisBacklinkCookie = "_gcl_au=1.1.1587853248.1631524319; _fbp=fb.1.1631524319197.41611897; hubspotutk=01b09236286543aedb83219e40df2c0e; __stripe_mid=f990c380-1fa2-4331-8c10-955f9df24b329d4a53; __stripe_mid=f990c380-1fa2-4331-8c10-955f9df24b329d4a53; _gid=GA1.2.1859426265.1635922654; __hssrc=1; mozauth=xLaZi2zMbZcXCjkHJgrVDSnZdybRmV7reCLj86O86PiJ7XrDXxAvhTawgPwej71e; ajs_user_id=18007615; ajs_anonymous_id=%22857dd900-057a-4341-af09-5ae8c52802f8%22; ajs_group_id=20017589; __cf_bm=eWGKexCMyzsev2LvlmBcB1cL7Q3uVdpj8noNlC2rjR4-1635924546-0-AVJMZ/7TxaafULewC+gSapER0BIz6Dg9SEm5CXF/eZ3nsZBIl5N5iaFffpgow1s8FQIPHucR6Y4lz7xXnSvQnNi5EG6uozSP7BTuVVRkrGwebRR/yit29DwBNA31F/i14gk8RPU2mS4aNVfZAoppB6u6rkNNztUeUGkBID/iQywK; __hstc=103427807.01b09236286543aedb83219e40df2c0e.1631524321393.1635922654472.1635924547640.16; __stripe_sid=7fa06247-a79f-41d8-a326-e57b92e22230d01c78; __stripe_sid=7fa06247-a79f-41d8-a326-e57b92e22230d01c78; _uetsid=527dcb103c7311eca24fa7d10d9105df; _uetvid=a61f8110147211ecbc6c83ec0baa15b8; _gat=1; _gat_UA-1870679-33=1; _gat_UA-1870679-27=1; _ga_LGQZKGRBE5=GS1.1.1635924546.17.1.1635924855.23; _ga=GA1.1.950776625.1631524319; __hssc=103427807.7.1635924547640; AWSALB=ZJj5F3XIyEhPpxqvvWSfXiGE7c9nOZSjirVmjXYhcbqPEYSOgrx4h7BkBBfuec9Sx2BMU62E8VWoqZK92OIUazLdwfmWm+xAudbIqdmiQ4G7MbegVM4iYmjyb/sU; AWSALBCORS=ZJj5F3XIyEhPpxqvvWSfXiGE7c9nOZSjirVmjXYhcbqPEYSOgrx4h7BkBBfuec9Sx2BMU62E8VWoqZK92OIUazLdwfmWm+xAudbIqdmiQ4G7MbegVM4iYmjyb/sU";

        public Dto.KeywordAnalysisDto AnalyisKeyword(string keyword, string locale="tr-TR", string engine="google")
        {
            var client = buildRestClient(analysisKeywordEndpoint);

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", analysisKeywordCookie);
            
            var body = "{\"engine\":\"" + engine + "\",\"keyword\":\"" + keyword + "\",\"locale\":\"" + locale + "\"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            try
            {
                IRestResponse response = client.Execute(request);

                var analysis = JsonConvert.DeserializeObject<Dto.KeywordAnalysisDto>(response.Content);

                return analysis;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        /// <summary>
        /// bilgekisi.com
        /// </summary>
        /// <param name="domain"></param>
        public Dto.BacklinkAnalysisDto AnalysisBacklink(string domain, string next_token="", int limit = 250)
        {
            var path = "https://analytics.moz.com/listerine/api/1.4/idina/links?site=" + domain + "&filter=external%2Bnot_deleted&limit=50&scope=pld&sort=source_page_authority&source_scope=pld&version=1";
            if (!string.IsNullOrEmpty(next_token))
                path += "&next_token=" + next_token;

            var client = buildRestClient(path);

            var request = new RestRequest(Method.GET);
            request.AddHeader("referer", "https://analytics.moz.com/pro/link-explorer/inbound-links?filterFeeds=false&filterOneLink=true&site=" + domain + "&state=not_deleted&target=domain");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Cookie", analysisBacklinkCookie);

            try
            {
                IRestResponse response = client.Execute(request);

                var analysis = JsonConvert.DeserializeObject<Dto.BacklinkAnalysisDto>(response.Content);
                if (!string.IsNullOrEmpty(analysis.Response.NextToken) && limit > 0)
                {
                    var nextPageResponse = AnalysisBacklink(domain, analysis.Response.NextToken, limit - 50);

                    analysis.Response.Links.AddRange(nextPageResponse.Response.Links);
                }

                return analysis;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        /// <summary>
        /// range içerisinde 0-99, 100-199 diye ilerliyor
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public Dto.SiteRankingsDto SiteRankings(string domain, int start = 0)
        {
            var client = buildRestClient(siteRankingsEndpoint);

            var request = new RestRequest(Method.POST);
            request.AddHeader("referer", "https://analytics.moz.com/");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", siteRankingsCookie);

            var body = "{\"filters\": {},\"result_range\": [" + start + "," + ((start + 99).ToString()) + "],\"sort\": {\"by\": \"primary_rank\",\"reverse\": false},\"subjects\": {\"primary\": {\"locale\": \"en-US\",\"scope\": \"domain\",\"target\": " +
                "\"" + domain + "\"},\"secondaries\": []},\"timeout_in_ms\": 60000,\"account_id\": " + accountId + "}";

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            try
            {
                IRestResponse response = client.Execute(request);

                var analysis = JsonConvert.DeserializeObject<Dto.SiteRankingsDto>(response.Content);
                return analysis;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        private RestClient buildRestClient(string path)
        {
            var client = new RestClient(path);
            client.Timeout = -1;
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36 Edg/92.0.902.73";

            return client;
        }
    }
}

using Newtonsoft.Json.Linq;
using RestSharp;
using SomeBlog.Integration.Ahrefs.Dto;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace SomeBlog.Integration.Ahrefs
{
    public class Service
    {
        private string Cookie;
        public Service(string cookie)
        {
            Cookie = cookie;
        }

        /// <summary>
        /// cookie almak icin bu sayfaya bak
        /// https://app.ahrefs.com/v2-site-explorer/organic-keywords/subdomains?compareDate=dontCompare&country=tr&target=hazirbilgi.net%2F
        /// cookie yi chrome'dan gidip alman gerekiyor, developer tools'dan bulabiliyorsun
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="page"></param>
        /// <param name="rowCount"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public GetOrganicKeywordsResponseDto GetOrganicKeywords(string domain, int page = 1, int rowCount = 50, string country = "tr")
        {
            var size = rowCount;
            var offset = (page - 1) * rowCount;
            var path = "https://app.ahrefs.com/v4/seGetOrganicKeywords?input=%7B%22params%22%3A%7B%22timeout%22%3Anull%2C%22shape%22%3A%5B%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22hash%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22text%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22serp_features%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22volume%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22volume_avg%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22difficulty%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22cpc%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22traffic%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22traffic_avg%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22traffic_alt%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22traffic_alt_avg%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22best_position%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22best_position_url%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22best_position_tkLinkContext_input%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22best_position_kind%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22best_position_has_thumbnail%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22best_position_has_video%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22target_positions_count%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22last_update%22%5D%7D%5D%7D%2C%7B%22f%22%3A%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22language%22%5D%7D%5D%7D%5D%2C%22drop_for_report__order_by%22%3A%5B%5B%22Desc%22%2C%5B%22Direct%22%2C%7B%22m%22%3A%22None%22%2C%22f%22%3A%5B%22Keywords%22%2C%22Keyword%22%2C%22traffic%22%5D%7D%5D%5D%5D%2C%22drop_for_report__offset%22%3A{offset}%2C%22drop_for_report__size%22%3A50%2C%22filter%22%3Anull%7D%2C%22args%22%3A%7B%22country%22%3A%22tr%22%2C%22reportMode%22%3A%5B%22Actual%22%2C%22Today%22%5D%2C%22multiTarget%22%3A%5B%22Single%22%2C%7B%22protocol%22%3A%22both%22%2C%22mode%22%3A%22subdomains%22%2C%22target%22%3A%22{domain}%2F%22%7D%5D%2C%22url%22%3A%22{domain}%2F%22%2C%22protocol%22%3A%22both%22%2C%22mode%22%3A%22subdomains%22%7D%7D";
            path = path.Replace("{domain}", domain);
            path = path.Replace("{offset}", offset.ToString());

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36");
            request.Headers.Add("cookie", Cookie);
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var responseContent = (response.Content.ReadAsStringAsync().Result);

            string errorMessage = "";
            if (responseContent.Contains("Unauthorized")) errorMessage = "Unauthorized";
            else if (responseContent.Contains("IsNotVerified")) errorMessage = "IsNotVerified";
            else if (responseContent.Contains("SeRowsPerReportLimitReached")) errorMessage = "Report Limit Reached";
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new GetOrganicKeywordsResponseDto()
                {
                    CurrentPage = page,
                    RowCount = 100,
                    Rows = new System.Collections.Generic.List<GetOrganicKeywordRowDto>(),
                    IsSucceed = false,
                    Message = errorMessage
                };
            }

            var indexOfFirstComma = responseContent.IndexOf(',');
            var content = responseContent.Substring(indexOfFirstComma + 1).TrimEnd(']');

            var jObj = JObject.Parse(content);
            var rows = JArray.Parse(jObj["rows"].ToString());

            var responseDto = new GetOrganicKeywordsResponseDto()
            {
                CurrentPage = page,
                Rows = new System.Collections.Generic.List<GetOrganicKeywordRowDto>(),
                RowCount = rowCount
            };

            /*
            
            0:0   {"f":["Direct",{"m":"None","f":["Keywords","Keyword","hash"]}]},
            1:1   {"f":["Direct",{"m":"None","f":["Keywords","Keyword","text"]}]},
            2:-1  {"f":["Direct",{"m":"None","f":["Keywords","Keyword","serp_features"]}]},  //bunu ucuruyoruz
            3:2   {"f":["Direct",{"m":"None","f":["Keywords","Keyword","volume"]}]},
            4:3   {"f":["Direct",{"m":"None","f":["Keywords","Keyword","volume_avg"]}]},
            5:4   {"f":["Direct",{"m":"None","f":["Keywords","Keyword","difficulty"]}]},
            6:5   {"f":["Direct",{"m":"None","f":["Keywords","Keyword","cpc"]}]},
            7:6   {"f":["Direct",{"m":"None","f":["Keywords","Keyword","traffic"]}]},
            8:7   {"f":["Direct",{"m":"None","f":["Keywords","Keyword","traffic_avg"]}]},
            9:8   {"f":["Direct",{"m":"None","f":["Keywords","Keyword","traffic_alt"]}]},
            10:9  {"f":["Direct",{"m":"None","f":["Keywords","Keyword","traffic_alt_avg"]}]},
            11:10 {"f":["Direct",{"m":"None","f":["Keywords","Keyword","best_position"]}]},
            12:11 {"f":["Direct",{"m":"None","f":["Keywords","Keyword","best_position_url"]}]},
            13:-1 {"f":["Direct",{"m":"None","f":["Keywords","Keyword","best_position_tkLinkContext_input"]}]}, //bunu da ucuruyoruz
            14:12 {"f":["Direct",{"m":"None","f":["Keywords","Keyword","best_position_kind"]}]},
            15:13 {"f":["Direct",{"m":"None","f":["Keywords","Keyword","best_position_has_thumbnail"]}]},
            16:14 {"f":["Direct",{"m":"None","f":["Keywords","Keyword","best_position_has_video"]}]},
            17:15 {"f":["Direct",{"m":"None","f":["Keywords","Keyword","target_positions_count"]}]},
            18:16 {"f":["Direct",{"m":"None","f":["Keywords","Keyword","last_update"]}]},
            19:17 {"f":["Direct",{"m":"None","f":["Keywords","Keyword","language"]}]}

             */
            foreach (var row in rows)
            {
                var strToSplit = row.ToString().Replace("\r\n", "")
                    .Replace(",  ", ",")
                    .Replace("  ", "");

                //strToSplit = Regex.Replace(strToSplit, @"\s", "");
                strToSplit = Regex.Replace(strToSplit, "^\\[", "");
                strToSplit = Regex.Replace(strToSplit, "]$", "");

                var splitStart = strToSplit.IndexOf(",[");
                var splitEnd = strToSplit.IndexOf("],") - splitStart + 1; //,[ + ], length
                strToSplit = strToSplit.Remove(splitStart, splitEnd);

                var splitStart2 = strToSplit.IndexOf("{");
                var splitEnd2 = strToSplit.IndexOf("}\",") - splitStart2 + 3; //,[ + ], length ve 1 tane de virgül için
                strToSplit = strToSplit.Remove(splitStart2, splitEnd2);

                strToSplit = strToSplit.Replace("\"", "");
                //language,best_position,last_update,best_position_url,cpc,best_position_has_video,best_position_kind,serp_target_positions_count_all,
                //sum_traffic,difficulty,text,best_position_has_thumbnail,hash,volume
                var arr = strToSplit.Split(',');

                //beklentimiz bu şekilde
                //gelen json'ı daha temiz parse etmeliyiz burada bi eksigimiz var
                //bu cok odunca oldu ama su an icin cozumumuz bu, 17 uzunlukta arr gelmezse bi yanlışlık var demektir
                //ornek olarak keyord su olabilir ankara, polatlı arası kaç km
                //bu da , split'te 1 item fazla getirmiş olabilir
                if (arr.Length != 18)
                    continue;

                var rowDto = new GetOrganicKeywordRowDto();

                rowDto.Keyword = arr[1];
                rowDto.Volume = int.Parse(arr[2]);
                rowDto.Difficulty = int.Parse(arr[4]);
                rowDto.Cpc = arr[5] == "null" ? 0 : int.Parse(arr[5]);
                rowDto.Traffic = int.Parse(arr[6]);
                rowDto.BestPosition = int.Parse(arr[10]);
                rowDto.Url = arr[11];
                rowDto.BestPositionKind = arr[12];
                rowDto.LastUpdate = DateTime.Parse(arr[16]);
                rowDto.Language = arr[17];

                responseDto.Rows.Add(rowDto);
            }

            responseDto.IsSucceed = true;
            responseDto.Message = "";

            return responseDto;
        }
    }
}

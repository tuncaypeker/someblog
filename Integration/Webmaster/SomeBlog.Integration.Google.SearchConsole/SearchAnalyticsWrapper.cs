using Google.Apis.SearchConsole.v1;
using Google.Apis.SearchConsole.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SomeBlog.Integration.Google.SearchConsole
{
    /// <summary>
    /// Bilgi alabilmek icin search console'da Kullanıcılar ve izinler ekranından service account email'e izin vermen lazim
    /// </summary>
    public class SearchAnalyticsWrapper
    {
        private readonly ServiceWrapper _connection;
        private readonly SearchConsoleService service;

        public SearchAnalyticsWrapper(string credentialsJson)
        {
            _connection = new ServiceWrapper(credentialsJson);
            service = _connection.GetWebmastersService();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="startDate">2021-08-13</param>
        /// <param name="endDate">2021-08-14</param>
        public List<Dto.SearchAnalyticsQueryDto> SearchAnalytics(string siteName, DateTime startDate, DateTime endDate, int rowLimit = 5000)
        {
            var req = new SearchAnalyticsQueryRequest()
            {
                StartDate = startDate.ToString("yyyy-MM-dd"),
                EndDate = endDate.ToString("yyyy-MM-dd"),
                Dimensions = new List<string>() {
                  "date",
                  "device",
                  "query",
                  "page",
                  "country"
                },
                RowLimit = rowLimit
            };

            var query = service.Searchanalytics.Query(req, siteName).Execute();
            if (query.Rows == null)
                return new List<Dto.SearchAnalyticsQueryDto>();

            return query.Rows.Select(x => Dto.SearchAnalyticsQueryDto.FromApiDataRow(x)).ToList();
        }

        public void Sites()
        {
            var list = service.Sites.List();
            var asd = list.Execute();
        }
    }
}

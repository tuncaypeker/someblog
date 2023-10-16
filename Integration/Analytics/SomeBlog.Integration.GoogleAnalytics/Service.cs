using Google.Analytics.Data.V1Beta;
using SomeBlog.Integration.GoogleAnalytics.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.GoogleAnalytics
{
    public class Service
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="properytyId">Google Analytics adres cubugunda p'den sonraki rakamlar</param>
        /// <param name="jsonCredentials">Google Cloud'dan indirilen json dosyasinin icerigi string olarak</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public List<VisitorSummaryDto> GetVisitorCounts(string propertyId, string jsonCredentials, DateTime startDate, DateTime endDate)
        {
            var clientBuilder = new BetaAnalyticsDataClientBuilder()
            {
                //CredentialsPath = "ServiceAccountCred.json",
                JsonCredentials = jsonCredentials
            };
            var client = clientBuilder.BuildAsync().Result;

            // Initialize request argument(s)
            RunReportRequest request = new RunReportRequest
            {
                Property = "properties/" + propertyId,
                Dimensions = { new Dimension { Name = "date" } },
                Metrics = {
                    new Google.Analytics.Data.V1Beta.Metric { Name = "totalUsers" },
                    new Google.Analytics.Data.V1Beta.Metric { Name = "newUsers" }
                },
                DateRanges = { new DateRange
                    {
                        StartDate = startDate.ToString("yyyy-MM-dd"), // "2023-01-01"
						EndDate = endDate.ToString("yyyy-MM-dd") // "2023-04-05" 
					}
                }
            };

            // Make the request
            var response = client.RunReport(request);

            var result = new List<VisitorSummaryDto>();

            foreach (Row row in response.Rows)
            {
                result.Add(new VisitorSummaryDto()
                {
                    Date = row.DimensionValues[0].Value,
                    TotalUsers = row.MetricValues[0].Value,
                    NewUsers = row.MetricValues[1].Value
                });
            }

            return result.OrderBy(x => x.Date).ToList();
        }
    }
}

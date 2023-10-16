using Google.Apis.SearchConsole.v1.Data;

namespace SomeBlog.Integration.Google.SearchConsole.Dto
{
    public class SearchAnalyticsQueryDto
    {
        public double Position { get; set; }
        public double Impressions { get; set; }
        public double Clicks { get; set; }
        public double Ctr { get; set; }
        public string Date { get; set; }
        public string Device { get; set; }
        public string Query { get; set; }
        public string Page { get; set; }
        public string Country { get; set; }

        public static SearchAnalyticsQueryDto FromApiDataRow(ApiDataRow apiDataRow)
        {
            /*
              "date",
              "device",
              "query",
              "page",
              "country"
             */
            return new SearchAnalyticsQueryDto()
            {
                Clicks = apiDataRow.Clicks.HasValue ? apiDataRow.Clicks.Value : 0,
                Country = apiDataRow.Keys[4],
                Ctr = apiDataRow.Ctr.HasValue ? apiDataRow.Ctr.Value : 0,
                Date = apiDataRow.Keys[0],
                Device = apiDataRow.Keys[1],
                Impressions = apiDataRow.Impressions.HasValue ? apiDataRow.Impressions.Value : 0,
                Page = apiDataRow.Keys[3],
                Position = apiDataRow.Position.HasValue ? apiDataRow.Position.Value : 0,
                Query = apiDataRow.Keys[2]
            };
        }
    }
}

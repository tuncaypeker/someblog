using BingWebmasterService;
using System;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class FetchedUrlDetailsDto
    {
        public DateTime Date { get; set; }
        public string Document { get; set; }
        public string Headers { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }

        public static FetchedUrlDetailsDto From(FetchedUrlDetails obj)
        {
            return new FetchedUrlDetailsDto()
            {
                Date = obj.Date,
                Document = obj.Document,
                Headers = obj.Headers,
                Status = obj.Status,
                Url = obj.Url
            };
        }
    }
}

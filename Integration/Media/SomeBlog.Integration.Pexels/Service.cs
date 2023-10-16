using SomeBlog.Integration.Pexels.Models;
using System;
using System.Linq;

namespace SomeBlog.Integration.Pexels
{
    public class Service
    {
        public SearchApiResult Search(string query, int page = 1)
        {
            //someblog58
            var pexelsClient = new PexelsClient("563492ad6f9170000100000134e5d4ff5d9e40cab65f69da9530826e");
            var result = pexelsClient.SearchPhotosAsync(query, page:page).Result;

            return new SearchApiResult()
            {
                TotalCount = result.totalResults,
                TotalPages = -1,
                Results = result.photos.Select(x => new Result()
                {
                    alt_description = "",
                    created_at = DateTime.Now.ToString(),
                    description = "",
                    height = x.height,
                    likes = 0,
                    width = x.width,
                    ImageUrl = new ImageUrl()
                    {
                        full = x.source.original,
                        raw = x.source.original,
                        regular = x.source.large2x,
                        small = x.source.medium,
                        thumb = x.source.small
                    }
                }).ToList()
            };
        }
    }
}

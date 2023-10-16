namespace SomeBlog.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;
    using SomeBlog.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FeedlyBlogData : EntityBaseData<Model.FeedlyBlog>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public FeedlyBlogData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
          : base(logger, serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public List<Model.FeedlyBlog> SearchSites(string query, string language, int page, int rowCount, string orderBy, bool isDesc)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                var queryable = _context.Set<Model.FeedlyBlog>().AsQueryable().Where(x => !x.CheckLater);

                if (!string.IsNullOrEmpty(query))
                    queryable = queryable.Where(x => x.Path.ToLower().Contains(query));

                if (!string.IsNullOrEmpty(language))
                    queryable = queryable.Where(x => x.Language.ToLower() == language);

                queryable = isDesc
                    ? queryable.OrderByDescending(orderBy)
                    : queryable.OrderBy(orderBy);

                queryable = queryable.Include(x => x.FeedlyBlogTopics).AsQueryable();

                return queryable.Skip((page - 1) * rowCount).Take(rowCount).ToList();
            }
        }

        public List<FeedlyBlog> GetByIds(int[] blogIds, int page, int rowCount)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                var queryable = _context.Set<Model.FeedlyBlog>()
                .Where(x => blogIds.Contains(x.Id))
                .Include(x => x.FeedlyBlogTopics)
                .Skip((page - 1) * rowCount).Take(rowCount).ToList();

                return queryable;
            }
        }
    }
}

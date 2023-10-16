namespace SomeBlog.Data.Pool
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using SomeBlog.Data.Infrastructure;

    public class PoolBlogData : EntityBaseData<Model.PoolBlog>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PoolBlogData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public List<Model.PoolBlog> SearchBlogs(string query, int poolLanguageId, int page, int rowCount, string orderBy = "Id", bool isDesc = true)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<PoolDataContext>();

                var queryable = _context.Set<Model.PoolBlog>().Include(x => x.PoolBlogSubjects).AsQueryable();
                if (!string.IsNullOrEmpty(query))
                    queryable = queryable.Where(x => x.Name.ToLower().Contains(query) || x.About.ToLower().Contains(query));

                if (poolLanguageId > 0)
                    queryable = queryable.Where(x => x.PoolLanguageId == poolLanguageId);

                queryable = isDesc
                    ? queryable.OrderByDescending(orderBy)
                    : queryable.OrderBy(orderBy);

                return queryable.Skip((page - 1) * rowCount).Take(rowCount).ToList();
            }
        }
    }
}

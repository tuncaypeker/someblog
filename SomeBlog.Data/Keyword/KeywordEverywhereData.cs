namespace SomeBlog.Data.Keyword
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;
    using System;
    using System.Linq;

    public class KeywordEverywhereData : EntityBaseData<Model.KeywordEverywhere>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KeywordEverywhereData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, IOptions<DatabaseSettings> dbOptions) : base(logger, serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public System.Collections.Generic.List<Model.KeywordEverywhere> GetByPageByIncludeKeyword(int pageNumber, int pageCount, string orderBy = "CreateDate", bool isDesc = true )
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                try
                {
                    var queryable = _context.Set<Model.KeywordEverywhere>().Include(x => x.Keyword).AsQueryable();

                    if (isDesc) queryable = queryable.OrderByDescending(orderBy);
                    else queryable = queryable.OrderBy(orderBy);

                    queryable = queryable.Skip((pageNumber - 1) * pageCount).Take(pageCount);

                    return queryable.ToList();
                }
                catch (Exception exc)
                {
                    return null;
                }
            }
        }
    }
}

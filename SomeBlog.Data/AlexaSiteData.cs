namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class AlexaSiteData : EntityBaseData<Model.AlexaSite>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AlexaSiteData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
          : base(logger, serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public List<Model.AlexaSite> SearchSites(string query, string country, int page, int rowCount, string orderBy, bool isDesc)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                var queryable = _context.Set<Model.AlexaSite>().AsQueryable();
                if (!string.IsNullOrEmpty(query))
                    queryable = queryable.Where(x => x.Path.ToLower().Contains(query));

                if (!string.IsNullOrEmpty(country))
                    queryable = queryable.Where(x => x.Country.ToLower().Contains(country));

                queryable = isDesc
                    ? queryable.OrderByDescending(orderBy)
                    : queryable.OrderBy(orderBy);

                return queryable.Skip((page - 1) * rowCount).Take(rowCount).ToList();
            }
        }
    }
}

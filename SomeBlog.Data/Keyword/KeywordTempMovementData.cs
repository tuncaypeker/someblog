namespace SomeBlog.Data.Keyword
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class KeywordTempMovementData : EntityBaseData<Model.KeywordTempMovement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KeywordTempMovementData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public bool TruncateTable()
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetService<DataContext>();

                _context.Database.ExecuteSqlCommand("TRUNCATE TABLE db_someblog.keyword_temp_movements");

                return true;
            }
        }

        public List<Model.KeywordTempMovement> GetMovementsWithKeyword(int blogId, string orderBy = "Id")
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                try
                {
                    return _context.Set<Model.KeywordTempMovement>()
                        .Where(x => x.BlogId == blogId)
                        .Include(x => x.Keyword)
                        .OrderByDescending(orderBy).ToList();
                }
                catch (Exception exc)
                {
                    return null;
                }
            }
        }
    }
}

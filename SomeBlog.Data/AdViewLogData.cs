namespace SomeBlog.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class AdViewLogData : EntityBaseData<Model.AdViewLog>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AdViewLogData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public bool TruncateTable()
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetService<DataContext>();

                _context.Database.ExecuteSqlCommand("TRUNCATE TABLE db_someblog.ad_view_logs");

                return true;
            }
        }
    }
}

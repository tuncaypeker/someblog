namespace SomeBlog.Data.Pool
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class PoolBlogImportRequestLogData : EntityBaseData<Model.PoolBlogImportRequestLog>
    {
        public PoolBlogImportRequestLogData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}

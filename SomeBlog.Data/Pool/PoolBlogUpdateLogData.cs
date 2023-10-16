namespace SomeBlog.Data.Pool
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class PoolBlogUpdateLogData : EntityBaseData<Model.PoolBlogUpdateLog>
    {
        public PoolBlogUpdateLogData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) {
        }
    }
}

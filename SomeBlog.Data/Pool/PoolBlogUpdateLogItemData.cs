namespace SomeBlog.Data.Pool
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class PoolBlogUpdateLogItemData : EntityBaseData<Model.PoolBlogUpdateLogItem>
    {
        public PoolBlogUpdateLogItemData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) {
        }
    }
}

namespace SomeBlog.Data.Pool
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class PoolCategoryData : EntityBaseData<Model.PoolCategory>
    {
        public PoolCategoryData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}

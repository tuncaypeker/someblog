namespace SomeBlog.Data.Pool
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class PoolContentCategoryData : EntityBaseData<Model.PoolContentCategory>
    {
        public PoolContentCategoryData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}

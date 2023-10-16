namespace SomeBlog.Data.Pool
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class PoolContentTranslateData : EntityBaseData<Model.PoolContentTranslate>
    {
        public PoolContentTranslateData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}

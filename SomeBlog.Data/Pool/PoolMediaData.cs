namespace SomeBlog.Data.Pool
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class PoolMediaData : EntityBaseData<Model.PoolMedia>
    {
        public PoolMediaData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}

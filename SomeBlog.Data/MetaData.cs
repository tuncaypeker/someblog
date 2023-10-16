namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class MetaData : EntityBaseData<Model.Meta>
    {
        public MetaData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}

namespace SomeBlog.Data.Pool
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class PoolBlogImportRequestRemoveRuleData : EntityBaseData<Model.PoolBlogImportRequestRemoveRule>
    {
        public PoolBlogImportRequestRemoveRuleData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}

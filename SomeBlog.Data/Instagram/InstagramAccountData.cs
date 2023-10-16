namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class InstagramAccountData : EntityBaseData<Model.InstagramAccount>
    {
        public InstagramAccountData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory): base(logger, serviceScopeFactory) { }
    }
}

namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class PinterestAccountData : EntityBaseData<Model.PinterestAccount>
    {
        public PinterestAccountData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory): base(logger, serviceScopeFactory) { }
    }
}

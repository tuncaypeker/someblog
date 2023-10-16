namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class AdBannedIpData : EntityBaseData<Model.AdBannedIp>
    {
        public AdBannedIpData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory): base(logger, serviceScopeFactory) { }
    }
}

namespace SomeBlog.Data.Keyword
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class KeywordSerpData : EntityBaseData<Model.KeywordSerp>
    {
        public KeywordSerpData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}

namespace SomeBlog.Data.Keyword
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;

    public class KeywordEverywhereQuestionData : EntityBaseData<Model.KeywordEverywhereQuestion>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KeywordEverywhereQuestionData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, IOptions<DatabaseSettings> dbOptions) : base(logger, serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
    }
}

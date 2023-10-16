namespace SomeBlog.Data.Content
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class ContentKeywordTailData : EntityBaseData<Model.ContentKeywordTail>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ContentKeywordTailData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) 
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }
    }
}

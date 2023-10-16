namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class FeedlyTopicRelatedData : EntityBaseData<Model.FeedlyTopicRelated>
    {
        public FeedlyTopicRelatedData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
          : base(logger, serviceScopeFactory) { }
    }
}

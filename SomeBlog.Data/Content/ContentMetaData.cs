namespace SomeBlog.Data.Content
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;
    using System.Collections.Generic;

    public class ContentMetaData : EntityBaseData<Model.ContentMeta>
    {
        public ContentMetaData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }

        public List<Model.ContentMeta> GetContentMetaByContentId(int id)
        {
            return GetBy(x => x.ContentId == id);
        }
    }
}

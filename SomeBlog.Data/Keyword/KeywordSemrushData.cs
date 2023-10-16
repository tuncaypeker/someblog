﻿namespace SomeBlog.Data.Keyword
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;

    public class KeywordSemrushData : EntityBaseData<Model.KeywordSemrush>
    {
        public KeywordSemrushData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, IOptions<DatabaseSettings> dbOptions) : base(logger, serviceScopeFactory)
        {
        }
    }
}

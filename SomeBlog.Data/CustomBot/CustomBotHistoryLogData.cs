namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class CustomBotHistoryLogData : EntityBaseData<Model.CustomBotHistoryLog>
    {
        public CustomBotHistoryLogData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
          : base(logger, serviceScopeFactory) { }
    }
}

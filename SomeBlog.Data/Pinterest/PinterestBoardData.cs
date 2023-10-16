namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class PinterestBoardData : EntityBaseData<Model.PinterestBoard>
    {
        public PinterestBoardData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory): base(logger, serviceScopeFactory) { }
    }
}

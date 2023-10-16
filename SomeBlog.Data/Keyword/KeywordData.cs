namespace SomeBlog.Data.Keyword
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class KeywordData : EntityBaseData<Model.Keyword>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KeywordData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, IOptions<DatabaseSettings> dbOptions) : base(logger, serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Model.Keyword GetByQuery(string query, bool asNoTracking = true)
        {
            return FirstOrDefault(x => x.Query == query, asNoTracking: asNoTracking);
        }

        public List<Model.Dto.KeywordJustIdKeywordDto> GetAllIdAndQuery()
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                var query = _context.Set<Model.Keyword>().Select(p => new Model.Dto.KeywordJustIdKeywordDto
                {
                    Id = p.Id,
                    Query = p.Query
                }).AsQueryable();

                return query.ToList();
            }
        }

        public List<Model.Keyword> GetContentKeywords(int contentId)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetService<DataContext>();
                var query = from k in dataContext.Keywords
                            join bg in dataContext.BlogKeywords on new { keyword = k.Id, contentId } equals new { keyword = bg.KeywordId, contentId = bg.ContentId }
                            select k;

                return query.ToList();
            }
        }
    }
}

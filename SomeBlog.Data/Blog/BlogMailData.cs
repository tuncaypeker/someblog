namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;

    public class BlogMailData : EntityBaseData<Model.BlogMail>
    {
        BlogMailHistoryData blogMailHistoryData;

        /// <summary>
        /// Silme islemi yapmayacaksan diger binding'lere gerek yok
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public BlogMailData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        {

        }

        public BlogMailData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, BlogMailHistoryData blogMailHistoryData) : base(logger, serviceScopeFactory)
        {
            this.blogMailHistoryData = blogMailHistoryData;
        }

        public DataResult DeleteWithAllRelationsByBlog(int blogId)
        {
            var blogMails = GetBy(x => x.BlogId == blogId);
            foreach (var blogMail in blogMails)
                DeleteWithAllRelations(blogMail.Id);

            return new DataResult(true, "");
        }

        public DataResult DeleteWithAllRelations(int id)
        {
            var dataResultHistory = blogMailHistoryData.DeleteBulk(x => x.BlogMailId == id);

            return DeleteByKey(id);
        }
    }
}

namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;

    public class JobLogData : EntityBaseData<Model.JobLog>
    {
        public JobLogData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
          : base(logger, serviceScopeFactory) { }

        JobLogItemData jobLogItemData;

        public JobLogData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, JobLogItemData jobLogItemData) : base(logger, serviceScopeFactory)
        {
            this.jobLogItemData = jobLogItemData;
        }

        public DataResult DeleteWithAllRelationsByBlog(int blogId)
        {
            var jobLogs = GetBy(x => x.BlogId == blogId);
            foreach (var jobLog in jobLogs)
                DeleteWithAllRelations(jobLog.Id);

            return new DataResult(true, "");
        }

        public DataResult DeleteWithAllRelations(int id)
        {
            var dataResultHistory = jobLogItemData.DeleteBulk(x => x.JobLogId == id);

            return DeleteByKey(id);
        }
    }
}

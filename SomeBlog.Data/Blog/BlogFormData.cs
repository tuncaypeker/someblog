namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;

    public class BlogFormData : EntityBaseData<Model.BlogForm>
    {
        BlogFormItemData blogFormItemData;
        BlogFormFillData blogFormFillData;
        BlogFormFillValueData blogFormFillValueData;

        /// <summary>
        /// Silme islemi yapmayacaksan diger binding'lere gerek yok
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public BlogFormData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        { 
            
        }

        public BlogFormData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, BlogFormItemData blogFormItemData, BlogFormFillData blogFormFillData, BlogFormFillValueData blogFormFillValueData)
          : base(logger, serviceScopeFactory)
        {
            this.blogFormItemData = blogFormItemData;
            this.blogFormFillData = blogFormFillData;
            this.blogFormFillValueData = blogFormFillValueData;
        }

        public DataResult DeleteWithAllRelationsByBlog(int blogId)
        {
            var blogForms = GetBy(x => x.BlogId == blogId);
            foreach (var blogForm in blogForms)
                DeleteWithAllRelations(blogForm.Id);

            return new DataResult(true, "");
        }

        public DataResult DeleteWithAllRelations(int id)
        {
            var dataResultItem = blogFormItemData.DeleteBulk(x => x.BlogFormId == id);
            var dataResultFill = blogFormFillData.DeleteBulk(x => x.BlogFormId == id);
            var dataResultFillValue = blogFormFillValueData.DeleteBulk(x => x.BlogFormId == id);

            return DeleteByKey(id);
        }
    }
}

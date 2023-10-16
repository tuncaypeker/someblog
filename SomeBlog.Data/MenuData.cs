namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;

    public class MenuData : EntityBaseData<Model.Menu>
    {
        MenuItemData menuItemData;

        public MenuData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }

        public MenuData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, MenuItemData menuItemData) : base(logger, serviceScopeFactory)
        {
            this.menuItemData = menuItemData;
        }

        public DataResult DeleteWithAllRelationsByBlog(int blogId)
        {
            var menus = GetBy(x => x.BlogId == blogId);
            foreach (var menu in menus)
                DeleteWithAllRelations(menu.Id);

            return new DataResult(true, "");
        }

        public DataResult DeleteWithAllRelations(int id)
        {
            var dataResultHistory = menuItemData.DeleteBulk(x => x.MenuId == id);

            return DeleteByKey(id);
        }
    }

}

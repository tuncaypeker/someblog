namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class ContactMessageData : EntityBaseData<Model.ContactMessage>
    {
        public ContactMessageData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}

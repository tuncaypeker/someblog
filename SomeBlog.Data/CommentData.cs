namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;
    using SomeBlog.Model.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommentData : EntityBaseData<Model.Comment>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CommentData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) 
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public List<CommentSimpleDto> GetContentComments(int content_id)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                var dtNow = DateTime.Now;

                var query = _context.Set<Model.Comment>()
                               .Where(p => p.ContentId == content_id && p.IsApproved && dtNow > p.Published)
                               .Select(p => new CommentSimpleDto()
                               {
                                   Created = p.Created.ToString("dd.MM.yyyy HH:mm"),
                                   Name = p.Name,
                                   Text = p.Text
                               }).AsQueryable();

                return query.ToList();
            }
        }
    }
}

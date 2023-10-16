namespace SomeBlog.Data.Pool
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PoolContentData : EntityBaseData<Model.PoolContent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        IOptions<DatabaseSettings> _dbOptions;
        SomeBlog.Infrastructure.Interfaces.ILogger<object> _logger;

        public PoolContentData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, IOptions<DatabaseSettings> dbOptions) : base(logger, serviceScopeFactory)
        {
            this._dbOptions = dbOptions;
            this._logger = logger;
            this._serviceScopeFactory = serviceScopeFactory;
        }

        /*
        
        CREATE DEFINER=`root`@`localhost` PROCEDURE `sm_get_a_pool_content_not_have_translation`(poolLanguageId int)
        BEGIN
        SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED ;

        set @script = concat('with trRecords as(
        select PoolContentId from db_someblog_pool.pool_content_translates where PoolLanguageId = "',poolLanguageId,'")
        SELECT PC.Id,PC.Title,PC.Content,PC.PoolLanguageId,PB.Name,PB.Id 
        FROM db_someblog_pool.pool_contents PC
        INNER JOIN db_someblog_pool.pool_blogs PB ON PC.PoolBlogId = PB.Id
        LEFT JOIN trRecords PCT ON PC.Id = PCT.PoolContentId
        WHERE pct.PoolContentId is null AND PC.IsDeleted = 0 AND PB.ShouldTranslateContents = 1 AND PC.CantTranslated = 0 AND PC.PoolLanguageId <> "', poolLanguageId ,'" LIMIT 0,1');

        prepare script from @script; 
        execute script;
        deallocate prepare script;

        SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ ;
        END

        ----
        
        CREATE DEFINER=`root`@`localhost` PROCEDURE `sm_get_a_pool_content_not_have_translation_in_blog`(poolLanguageId int, blogId int)
        BEGIN
        SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED ;

        set @script = concat('with trRecords as(
        select PoolContentId from db_someblog_pool.pool_content_translates where PoolLanguageId = "',poolLanguageId,'")
        SELECT PC.Id,PC.Title,PC.Content,PC.PoolLanguageId,PB.Name,PB.Id 
        FROM db_someblog_pool.pool_contents PC
        INNER JOIN db_someblog_pool.pool_blogs PB ON PC.PoolBlogId = PB.Id
        LEFT JOIN trRecords PCT ON PC.Id = PCT.PoolContentId
        WHERE pct.PoolContentId is null AND PC.PoolBlogId=',blogId,' AND PC.IsDeleted = 0 AND PB.ShouldTranslateContents = 1 AND PC.CantTranslated = 0 AND PC.PoolLanguageId <> "', poolLanguageId ,'" LIMIT 0,1');

        prepare script from @script; 
        execute script;
        deallocate prepare script;

        SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ ;
        END

         */
        public Model.Dto.PoolContentTranslateSimpleDto GetThatNotHaveTranslate(int poolLanguageId, int blogId = -1)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<PoolDataContext>();

                using (var conn = _context.Database.GetDbConnection())
                {
                    try
                    {
                        conn.Open();
                        var command = conn.CreateCommand();
                        var query = blogId == -1
                            ? $"call db_someblog_pool.sm_get_a_pool_content_not_have_translation('" + poolLanguageId + "')"
                            : $"call db_someblog_pool.sm_get_a_pool_content_not_have_translation_in_blog('" + poolLanguageId + "'," + blogId + ")";
                        command.CommandText = query;
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var id = reader.GetInt32(0);
                            var title = reader.GetString(1);
                            var content = reader.GetString(2);
                            var poolLanguage = reader.GetInt32(3);
                            var blogName = reader.GetString(4);
                            var blogIdFromDb = reader.GetInt32(5);

                            return new Model.Dto.PoolContentTranslateSimpleDto()
                            {
                                Content = content,
                                Id = id,
                                Title = title,
                                PoolLanguageId = poolLanguage,
                                BlogName = blogName,
                                BlogId = blogIdFromDb
                            };
                        }

                        return null;
                    }
                    catch
                    {
                        return null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        /*
         
        CREATE DEFINER=`root`@`localhost` PROCEDURE `sm_get_pool_contents_with_page_bigger_than_id`(lastId int, count int)
        BEGIN
        SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED ;

        set @script = concat('SELECT Id,PoolBlogId,SiteKeyId,Content FROM db_someblog_pool.pool_contents where Id > ',lastId,' ORDER BY Id LIMIT 0,',count,';');

        prepare script from @script; 
        execute script;
        deallocate prepare script;

        SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ ;
        END

        */
        /// <summary>
        /// Media Process botu kullanıyor, sonrasında kaldırılabilir, Bu yuzden where sorgusunda HasMediaProcessed flag'i ekli
        /// </summary>
        /// <param name="lastId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public System.Collections.Generic.List<Model.PoolContent> GetWithPageBiggerThanId(int lastId, int count = 100)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<PoolDataContext>();

                using (var conn = _context.Database.GetDbConnection())
                {
                    try
                    {
                        conn.Open();
                        var command = conn.CreateCommand();
                        var query = $"call db_someblog_pool.sm_get_pool_contents_with_page_bigger_than_id(" + lastId + "," + count + ")";
                        command.CommandText = query;
                        var reader = command.ExecuteReader();
                        var list = new System.Collections.Generic.List<Model.PoolContent>();

                        while (reader.Read())
                        {
                            var id = reader.GetInt32(0);
                            var poolBlogId = reader.GetInt32(1);
                            var siteKeyId = reader.GetString(2);
                            var content = reader.GetString(3);

                            list.Add(new Model.PoolContent()
                            {
                                Content = content,
                                Id = id,
                                SiteKeyId = siteKeyId,
                                PoolBlogId = poolBlogId
                            });
                        }

                        return list;
                    }
                    catch (Exception exc)
                    {
                        return null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public DataResult UpdatePageTitleTr(int poolContentId, string titleTR)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<PoolDataContext>();

                try
                {
                    var user = new Model.PoolContent() { Id = poolContentId, TitleTR = titleTR };
                    using (var db = new PoolDataContext(_dbOptions.Value.PoolConnectionString))
                    {
                        db.Set<Model.PoolContent>().Attach(user);
                        db.Entry(user).Property(x => x.TitleTR).IsModified = true;
                        db.SaveChanges();
                    }

                    return new DataResult(true, "");
                }
                catch (Exception exc)
                {
                    _logger.Error(string.Format(InsertFailedLogTemplate, typeof(Model.PoolContent), exc.Message));

                    return new DataResult(false, exc.Message +
                        exc.InnerException == null ? "" : "(" + exc.InnerException + ")"
                    );
                }
            }
        }

        public DataResult UpdateCantTranslated(int poolContentId, bool cantTranslated)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<PoolDataContext>();

                try
                {
                    var user = new Model.PoolContent() { Id = poolContentId, CantTranslated = cantTranslated };
                    using (var db = new PoolDataContext(_dbOptions.Value.PoolConnectionString))
                    {
                        db.Set<Model.PoolContent>().Attach(user);
                        db.Entry(user).Property(x => x.CantTranslated).IsModified = true;
                        db.SaveChanges();
                    }

                    return new DataResult(true, "");
                }
                catch (Exception exc)
                {
                    _logger.Error(string.Format(InsertFailedLogTemplate, typeof(Model.PoolContent), exc.Message));

                    return new DataResult(false, exc.Message +
                        exc.InnerException == null ? "" : "(" + exc.InnerException + ")"
                    );
                }
            }
        }

        public DataResult UpdateHasMediaProcessed(int poolContentId, bool hasMediaProcessed)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<PoolDataContext>();

                try
                {
                    var poolContent = new Model.PoolContent() { Id = poolContentId, HasMediaProcessed = hasMediaProcessed };
                    using (var db = new PoolDataContext(_dbOptions.Value.PoolConnectionString))
                    {
                        db.Set<Model.PoolContent>().Attach(poolContent);
                        db.Entry(poolContent).Property(x => x.HasMediaProcessed).IsModified = true;
                        db.SaveChanges();
                    }

                    return new DataResult(true, "");
                }
                catch (Exception exc)
                {
                    _logger.Error(string.Format(InsertFailedLogTemplate, typeof(Model.PoolContent), exc.Message));

                    return new DataResult(false, exc.Message +
                        exc.InnerException == null ? "" : "(" + exc.InnerException + ")"
                    );
                }
            }
        }

        public System.Collections.Generic.List<Model.PoolContent> GetWithSkipAndTake(int poolBlogId, int skip, int take, string orderBy)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<PoolDataContext>();

                return _context.Set<Model.PoolContent>()
                    .Where(x => !x.IsDeleted && x.PoolBlogId == poolBlogId)
                    .OrderBy(orderBy)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            }
        }

        public List<Model.PoolContent> QueryWithCategories(int poolBlogId, string query, int categoryId, int page, int rowCount, string orderBy, bool isDesc)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;

                var _context = scope.ServiceProvider.GetService<PoolDataContext>();

                var queryable = _context.Set<Model.PoolContent>().AsQueryable();

                queryable = (!string.IsNullOrEmpty(query))
                    ? queryable.Where(x => (x.Title.Contains(query) || x.TitleTR.Contains(query)) && x.PoolBlogId == poolBlogId && !x.IsDeleted)
                    : queryable.Where(x => x.PoolBlogId == poolBlogId && !x.IsDeleted);

                queryable = isDesc
                    ? queryable.OrderByDescending(orderBy).AsQueryable()
                    : queryable.OrderBy(orderBy);

                queryable = queryable.Include(x => x.PoolContentCategories);

                if (categoryId > 0) 
                    queryable = queryable.Where(x => x.PoolContentCategories.Any(y => y.PoolCategoryId == categoryId));

                return queryable
                    .Skip((page - 1) * rowCount).Take(rowCount)
                    .ToList();
            }
        }
    }
}

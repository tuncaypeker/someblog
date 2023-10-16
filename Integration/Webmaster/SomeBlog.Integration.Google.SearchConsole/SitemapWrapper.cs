using Google.Apis.SearchConsole.v1;
using SomeBlog.Integration.Google.SearchConsole.Dto;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SomeBlog.Integration.Google.SearchConsole
{
    public class SitemapWrapper
    {
        private readonly ServiceWrapper _connection;
        private readonly SearchConsoleService service;

        public SitemapWrapper(string credentialsJson)
        {
            _connection = new ServiceWrapper(credentialsJson);
            service = _connection.GetWebmastersService();
        }

        public List<SitemapsListDto> List(string siteName)
        {
            var queryResult = service.Sitemaps.List(siteName).Execute();
            return queryResult.Sitemap.Select(x => new SitemapsListDto()
            {
                LastDownloaded = DateTime.Parse(x.LastDownloaded.ToString()),
                LastSubmitted = DateTime.Parse(x.LastSubmitted.ToString()),
                Path = x.Path,
                Warnings = x.Warnings,
                IsPending = x.IsPending.Value,
                IsSitemapsIndex = x.IsSitemapsIndex.Value
            }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteName">https://blogpath.com</param>
        /// <param name="feedPath">https://blogpath.com/sitemap.xml</param>
        public bool Submit(string siteName,string feedPath)
        {
            try
            {
                var resource = service.Sitemaps.Submit(siteName, feedPath);
                var response = resource.Execute();

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
    }
}

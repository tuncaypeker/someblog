using System;
using System.Collections.Generic;
using System.Net;

namespace SomeBlog.Audit
{
    public class Basics
    {
        private static string _category = "Basics";
        public static List<Dto.AuditResultItem> RunAudits(Model.Blog blog)
        {
            var auditResults = new List<Dto.AuditResultItem>() {
                HasRobotsTxt(blog),
                HasSitemap(blog)
            };

            return auditResults;
        }

        public static Dto.AuditResultItem HasRobotsTxt(Model.Blog blog)
        {
            var result = new Dto.AuditResultItem(_category, "has_robots_txt", hasPassed: false);
            try
            {
                var path = blog.Url.TrimEnd('/') + "/robots.txt";
                using (var client = new WebClient())
                {
                    var content = client.DownloadString(path);
                }

                result.HasPassed = true;
            }
            catch (Exception exc)
            {
                result.Message = exc.Message;
            }

            return result;
        }

        public static Dto.AuditResultItem HasSitemap(Model.Blog blog)
        {
            var result = new Dto.AuditResultItem(_category, "has_sitemap_xml", hasPassed: false);
            try
            {
                var path = blog.Url.TrimEnd('/') + "/sitemap.xml";
                using (var client = new WebClient())
                {
                    var content = client.DownloadString(path);
                }

                result.HasPassed = true;
            }
            catch (Exception exc)
            {
                result.Message = exc.Message;
            }

            return result;
        }
    }
}

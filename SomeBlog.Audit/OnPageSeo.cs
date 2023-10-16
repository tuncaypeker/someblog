using System;
using System.Collections.Generic;
using System.Net;

namespace SomeBlog.Audit
{
    public class OnPageSeo
    {
        private static string _category = "OnPageSeo";
        public static List<Dto.AuditResultItem> RunAudits(Model.Content content)
        {
            var auditResults = new List<Dto.AuditResultItem>() {
                HasFocusKeyword(content),
                HasShortUrl(content),
                HasMetaDescription(content)
            };

            return auditResults;
        }


        /// <summary>
        /// Content Focus Keyword must set
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Dto.AuditResultItem HasFocusKeyword(Model.Content content)
        {
            var result = new Dto.AuditResultItem(_category, "has_focus_keyword");

            result.HasPassed = !string.IsNullOrEmpty(content.FocusKeyword);
            result.Message = result.HasPassed
                ? "Has Focus Keyword"
                : "Has no Focus Keyword";

            return result;
        }

        /// <summary>
        /// length of Slug must be smaller than 60
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Dto.AuditResultItem HasShortUrl(Model.Content content)
        {
            var result = new Dto.AuditResultItem(_category, "has_short_url");

            result.HasPassed = content.Slug.Length < 60;
            result.Message = result.HasPassed
                ? "Has Short Url"
                : "Has no Short Url";

            return result;
        }

        public static Dto.AuditResultItem HasMetaDescription(Model.Content content)
        {
            var result = new Dto.AuditResultItem(_category, "has_meta_description");

            result.HasPassed = !string.IsNullOrEmpty(content.MetaDescription);
            result.Message = result.HasPassed
                ? "Has Focus Keyword"
                : "Has no Focus Keyword";

            return result;
        }
    }
}

namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Blog : Core.ModelBase
    {
        public Blog()
        {
            CreatedTime = DateTime.Now;
            ModifiedTime = DateTime.Now;
        }

        public int AdAccountId { get; set; }
        public int DomainId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; } //http:...
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

        public string IpAddress { get; set; }
        public string FtpAddress { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }
        public int FtpPort { get; set; }

        /// <summary>
        /// wp-content, uploads vs.
        /// </summary>
        public string MediaBasePath { get; set; }

        public string HeaderCode { get; set; }
        public string FooterCode { get; set; }

        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Pinterest { get; set; }
        public string Twitter { get; set; }
        public string Youtube { get; set; }

        public string HomeMetaTitle { get; set; }
        public string HomeMetaDescription { get; set; }

        public DateTime SslFinishDate { get; set; }
        public DateTime DomainFinishDate { get; set; }

        public bool HasSuspended { get; set; }
        
        public int GoogleAccountId { get; set; }
        public int BingAccountId { get; set; }
        public int YandexAccountId { get; set; }
        public int AhrefsAccountId { get; set; }
        public int GoogleNewsAccountId { get; set; }

        public int ThemeId { get; set; }
        public string ThemeVersion { get; set; }

        public string DeploymentVersion { get; set; }
        public string ContentLinkTemplate { get; set; }

        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
        public string Pop3Address { get; set; }
        public int Pop3Port { get; set; }

        public string GoogleAnalyticsSiteId { get; set; }
        public string GoogleServiceCredentials { get; set; }

        /// <summary>
        /// https://www.hazirbilgi.net/(.*).html
        /// </summary>
        public string GetSlugRegex { get; set; }

        /// <summary>
        /// https://www.indexnow.org/
        /// </summary>
        public string IndexNowKey { get; set; }

        /// <summary>
        /// Bu flag ile audit botu, backup botu, seo platform botları calısmaz
        /// </summary>
        public bool IsDemoBlog { get; set; }
        public bool HasGoogleNews { get; set; }

        public string CultureName { get; set; }

        //https://www.robingupta.com/bulk-domain-authority-checker.html
        public int DomainAuthority { get; set; }
        public int PageAuthority { get; set; }
        public int SpamScore { get; set; }

        //tema dosyasında 
        public string CustomCss { get; set; }

        [NotMapped, Ignored]
        public bool HasFtp
        {
            get
            {
                return FtpPassword != null &&
                    FtpUserName != null &&
                    FtpAddress != null;
            }
        }

        /// <summary>
        /// Tek iis process uzerinden cevap veren bloglardan biri mi
        /// </summary>
        public bool HasBundleUI { get; set; }
    }
}

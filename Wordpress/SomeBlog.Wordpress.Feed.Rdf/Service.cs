using Newtonsoft.Json;
using SomeBlog.Wordpress.Feed.Rdf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;

namespace SomeBlog.Wordpress.Feed.Rdf
{
    public class Service
    {
        private string _basePath;

        /// <summary>
        /// "http://google.com/";
        /// </summary>
        public Service(string basePath)
        {
            _basePath = basePath.TrimEnd('/');
        }

        public List<Post> GetPosts()
        {
            var path = $"{_basePath}/feed/rdf";

            using (var client = new WebClient())
            {
                var str = "";

                try
                {
                    str = client.DownloadString(path);
                }
                catch (Exception exc) {
                    return null;
                }

                if (string.IsNullOrEmpty(str))
                    return null;

                str = str.Replace("/*", "")
                    .Replace("*/", "")
                    .Replace("<!--", "")
                    .Replace("-->", "");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(str);

                string jsonStr = JsonConvert.SerializeXmlNode(doc);
                var post = JsonConvert.DeserializeObject<RdfResponseDto>(jsonStr);

                var posts = post.RdfRDF.item.Select(x => new Post {
                    Content = x.ContentEncoded != null
                        ? x.ContentEncoded.CdataSection
                        : "",
                    PublishDate = x.DcDate,
                    Categories = x.DcSubject.ToString().StartsWith('[')
                        ? JsonConvert.DeserializeObject<List<CategoryEncoded>>(x.DcSubject.ToString()).Select(x => x.CdataSection).ToList()
                        : new List<string>() { JsonConvert.DeserializeObject<CategoryEncoded>(x.DcSubject.ToString()).CdataSection }
                    ,
                    Summary = x.description.CdataSection,
                    Link = x.link,
                    Title = x.title,
                }).ToList();

                return posts;
            }
        }
    }
}

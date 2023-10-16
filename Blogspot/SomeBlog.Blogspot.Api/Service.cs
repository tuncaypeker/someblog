using Google.Apis.Auth.OAuth2;
using Google.Apis.Blogger.v3;
using Google.Apis.Services;
using Newtonsoft.Json;
using System;
using System.Net;

namespace SomeBlog.Blogspot.Api
{
    public class Service
    {
        public Dto.Posts GetPosts(string siteId)
        { 
            var post_count = "500";
            var api_key = "AIzaSyDExPtXYsIUJpLAyzG8koYaaYeo4o7-tW0";

            var url = $"https://www.googleapis.com/blogger/v3/blogs/{siteId}/posts?key={api_key}&maxResults={post_count}";

            var json_text = "";
            using (WebClient wc = new WebClient())
            {
                json_text = wc.DownloadString(url);
            }

            if (!string.IsNullOrEmpty(json_text))
            {
                var json_model = JsonConvert.DeserializeObject<Dto.Posts>(json_text);
                return json_model;
            }

            return null;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/19244654/how-to-use-blogger-api-v3
        /// 
        /// !!!!! BLOGGER Service account desteklemiyor diye okudum bi yerde aq api key'de desteklemiyor sanki blog alıyorum ama post yapmıyor
        /// genis bi zamanda su oauth olayını cozmek lazim, adsense'de ayni mevzu var
        /// </summary>
        public void Test(string json)
        {
            BloggerService service = new BloggerService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyDExPtXYsIUJpLAyzG8koYaaYeo4o7-tW0",
            });

            string[] _scopes = {
                BloggerService.Scope.Blogger,
                BloggerService.Scope.BloggerReadonly
            };

            var credential = GoogleCredential.FromJson(json)
               .CreateScoped(_scopes)
               .UnderlyingCredential as ServiceAccountCredential;

            var blogResult = service.Blogs.GetByUrl("https://tuncaypeker1.blogspot.com/").ExecuteAsync().Result;

            /*
            // Display the results.
            if (blogResult.Posts != null)
            {
                //Run the posts request
                Console.WriteLine($"Executing posts {blogResult.Posts.SelfLink} request...");
                var postsResult = service.Posts.List(blogResult.Id).ExecuteAsync().Result;

                foreach (var post in postsResult.Items)
                {
                    Console.WriteLine($"{post.Id} - {post.Title}");
                }
            }
            */

            var insert = service.Posts.Insert(new Google.Apis.Blogger.v3.Data.Post()
            {
                Content = "test",
                CustomMetaData = "buraya custom meta data mi geliyor",
                Title = "Konu basligi buraya geliyor",
                Blog = new Google.Apis.Blogger.v3.Data.Post.BlogData()
                {
                    Id = blogResult.Id
                },
                Kind = "blogger#post",
            }, "2880871025310601235");

            var asd = insert.Execute();
        }
    }
}

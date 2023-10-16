using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace SomeBlog.Integration.Yandex.SearchImage
{
    public class Service
    {
        public Dto.ImageResultDto GetImageResults(string query)
        {
            var result = new Dto.ImageResultDto() { IsSucceed = false, Images = new List<Dto.ImageDto>() };

            using (var client = new WebClient())
            {
                var path = $"https://yandex.com.tr/images/search?text={query}";
                var html = client.DownloadString(path);

                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(html);

                var imageNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class,'serp-list')]/div[contains(@class,'serp-item')]");
                if (imageNodes == null)
                {
                    result.Message = "Image Nodes bulunamadı";
                    return result;
                }

                foreach (var imageNode in imageNodes)
                {
                    var attrM = imageNode.Attributes["data-bem"];
                    if (attrM == null)
                        continue;

                    var valueJson = attrM.Value;
                    var image = JsonConvert.DeserializeObject<Dto.ImageDto>(valueJson);

                    result.Images.Add(image);
                }

                if (imageNodes.Count > 0 && result.Images.Count == 0)
                {
                    result.Message = "Resimleri Parse edemiyorum";
                    return result;
                }

                result.IsSucceed = true;
                result.Message = "";

                return result;
            }
        }
    }
}

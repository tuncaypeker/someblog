using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace SomeBlog.Integration.Bing.SearchImage
{
    public class Service
    {
        public Dto.ImageResultDto GetImageResults(string query)
        {
            var result = new Dto.ImageResultDto() { IsSucceed = false, Images = new List<Dto.ImageDto>() };

            using (var client = new WebClient())
            {
                //var path = $"https://www.bing.com/images/search?q={query}&form=HDRSC2&first=1&tsc=ImageBasicHover";
                var path = $"https://www.bing.com/images/search?tsc=ImageBasicHover&q={query}&qft=+filterui:imagesize-large+filterui:age-lt525600&form=IRFLTR&first=1";
                var html = client.DownloadString(path);

                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(html);

                var imageNodes = htmlDoc.DocumentNode.SelectNodes("//ul[@class='dgControl_list ']/li/div/div/a");
                if (imageNodes == null)
                {
                    result.Message = "Image Nodes bulunamadı";
                    return result;
                }

                foreach (var imageNode in imageNodes)
                {
                    var attrM = imageNode.Attributes["m"];
                    if (attrM == null)
                        continue;

                    var valueJson = attrM.Value.Replace("&quot;", "\"");
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

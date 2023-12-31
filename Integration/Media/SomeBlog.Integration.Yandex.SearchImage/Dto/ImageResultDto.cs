﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.Yandex.SearchImage.Dto
{
    public class ImageResultDto
    {
        public List<ImageDto> Images { get; set; }
        public string Message { get; set; }
        public bool IsSucceed { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Preview
    {
        public string url { get; set; }
        public int fileSizeInBytes { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }

    public class Dup
    {
        public string url { get; set; }
        public int fileSizeInBytes { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }

    public class Size
    {
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Thumb
    {
        public string url { get; set; }
        public Size size { get; set; }
    }

    public class Snippet
    {
        public string title { get; set; }
        public bool hasTitle { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public string domain { get; set; }
        public int shopScore { get; set; }
    }

    public class SerpItem
    {
        public string reqid { get; set; }
        public string freshness { get; set; }
        public List<Preview> preview { get; set; }
        public List<Dup> dups { get; set; }
        public Thumb thumb { get; set; }
        public Snippet snippet { get; set; }
        public string detail_url { get; set; }
        public string img_href { get; set; }
        public bool useProxy { get; set; }
        public int pos { get; set; }
        public string id { get; set; }
        public string rimId { get; set; }
        public string docid { get; set; }
        public bool isMarketIncut { get; set; }
        public string counterPath { get; set; }
    }

    public class ImageDto
    {
        [JsonProperty("serp-item")]
        public SerpItem SerpItem { get; set; }
    }


}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SomeBlog.Integration.Google.GoogleTrends.Dto
{
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(Rss));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (Rss)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "link")]
    public class Link
    {

        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }

        [XmlAttribute(AttributeName = "rel")]
        public string Rel { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "news_item")]    
    public class NewsItem
    {

        [XmlElement(ElementName = "news_item_title")]
        public string NewsItemTitle { get; set; }

        [XmlElement(ElementName = "news_item_snippet")]
        public string NewsItemSnippet { get; set; }

        [XmlElement(ElementName = "news_item_url")]
        public string NewsItemUrl { get; set; }

        [XmlElement(ElementName = "news_item_source")]
        public string NewsItemSource { get; set; }
    }

    [XmlRoot(ElementName = "item")]
    public class Item
    {

        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "approx_traffic")]
        public string ApproxTraffic { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "link")]
        public string Link { get; set; }

        [XmlElement(ElementName = "pubDate")]
        public string PubDate { get; set; }

        [XmlElement(ElementName = "picture")]
        public string Picture { get; set; }

        [XmlElement(ElementName = "picture_source")]
        public string PictureSource { get; set; }

        [XmlElement(ElementName = "news_item")]
        public List<NewsItem> NewsItem { get; set; }
    }

    [XmlRoot(ElementName = "channel")]
    public class Channel
    {

        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "link")]
        public List<string> Link { get; set; }

        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
    }

    [XmlRoot(ElementName = "rss")]
    public class TrendingSearchRssDto
    {

        [XmlElement(ElementName = "channel")]
        public Channel Channel { get; set; }

        [XmlAttribute(AttributeName = "atom")]
        public string Atom { get; set; }

        [XmlAttribute(AttributeName = "ht")]
        public string Ht { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public double Version { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}

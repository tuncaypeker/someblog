using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SomeBlog.Wordpress.Feed.Rdf.Model
{
    public class Xml
    {
        [JsonProperty("@version")]
        public string Version { get; set; }

        [JsonProperty("@encoding")]
        public string Encoding { get; set; }
    }

    public class RdfLi
    {
        [JsonProperty("@rdf:resource")]
        public string RdfResource { get; set; }
    }

    public class RdfSeq
    {
        [JsonProperty("rdf:li")]
        public List<RdfLi> RdfLi { get; set; }
    }

    public class Items
    {
        [JsonProperty("rdf:Seq")]
        public RdfSeq RdfSeq { get; set; }
    }

    public class Channel
    {
        [JsonProperty("@rdf:about")]
        public string RdfAbout { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }

        [JsonProperty("dc:date")]
        public string DcDate { get; set; }

        [JsonProperty("sy:updatePeriod")]
        public string SyUpdatePeriod { get; set; }

        [JsonProperty("sy:updateFrequency")]
        public string SyUpdateFrequency { get; set; }

        [JsonProperty("sy:updateBase")]
        public string SyUpdateBase { get; set; }

        [JsonProperty("#text")]
        public string Text { get; set; }
        public Items items { get; set; }
    }

    public class DcCreator
    {
        [JsonProperty("#cdata-section")]
        public string CdataSection { get; set; }
    }

    public class Description
    {
        [JsonProperty("#cdata-section")]
        public string CdataSection { get; set; }
    }

    public class ContentEncoded
    {
        [JsonProperty("#cdata-section")]
        public string CdataSection { get; set; }
    }

    public class CategoryEncoded
    {
        [JsonProperty("#cdata-section")]
        public string CdataSection { get; set; }
    }

    public class Item
    {
        [JsonProperty("@rdf:about")]
        public string RdfAbout { get; set; }
        public string title { get; set; }
        public string link { get; set; }

        [JsonProperty("dc:creator")]
        public DcCreator DcCreator { get; set; }

        [JsonProperty("dc:date")]
        public DateTime DcDate { get; set; }

        [JsonProperty("dc:subject")]
        public object DcSubject { get; set; }
        public Description description { get; set; }

        [JsonProperty("content:encoded")]
        public ContentEncoded ContentEncoded { get; set; }
    }

    public class RdfRDF
    {
        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }

        [JsonProperty("@xmlns:rdf")]
        public string XmlnsRdf { get; set; }

        [JsonProperty("@xmlns:dc")]
        public string XmlnsDc { get; set; }

        [JsonProperty("@xmlns:sy")]
        public string XmlnsSy { get; set; }

        [JsonProperty("@xmlns:admin")]
        public string XmlnsAdmin { get; set; }

        [JsonProperty("@xmlns:content")]
        public string XmlnsContent { get; set; }
        public Channel channel { get; set; }
        public List<Item> item { get; set; }
    }

    public class RdfResponseDto
    {
        [JsonProperty("?xml")]
        public Xml Xml { get; set; }

        [JsonProperty("rdf:RDF")]
        public RdfRDF RdfRDF { get; set; }
    }
}

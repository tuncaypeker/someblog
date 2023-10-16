using CookComputing.XmlRpc;

namespace SomeBlog.Wordpress.XmlRpc.Models
{
    public class Enclosure
    {
        [XmlRpcMember("url")]
        public string Url { get; set; }

        [XmlRpcMember("length")]
        public int Length { get; set; }

        [XmlRpcMember("type")]
        public string Type { get; set; }
    }
}

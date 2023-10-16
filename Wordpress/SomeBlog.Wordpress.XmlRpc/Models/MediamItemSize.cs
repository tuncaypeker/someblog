using CookComputing.XmlRpc;

namespace SomeBlog.Wordpress.XmlRpc.Models
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class MediaItemSize
    {
        [XmlRpcMember("file")]
        public string File { get; set; }
        
        [XmlRpcMember("width")]
        public int Width { get; set; }
        
        [XmlRpcMember("height")]
        public int Height { get; set; }
        
        [XmlRpcMember("mime-type")]
        public string MimeType { get; set; }
    }
}

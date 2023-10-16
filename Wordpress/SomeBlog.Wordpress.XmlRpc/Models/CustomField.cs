using System.Data.Common;
using CookComputing.XmlRpc;

namespace SomeBlog.Wordpress.XmlRpc.Models
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class CustomField
    {
        [XmlRpcMember("id")]
        public string Id { get; set; }

        [XmlRpcMember("key")]
        public string Key { get; set; }

        [XmlRpcMember("value")]
        public object Value { get; set; }
    }
}

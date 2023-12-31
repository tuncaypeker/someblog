﻿using CookComputing.XmlRpc;

namespace SomeBlog.Wordpress.XmlRpc.Models
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class CommentFilter : FilterBase
    {
        [XmlRpcMember("post_id")]
        public int PostId { get; set; }

        [XmlRpcMember("status")]
        public string Status { get; set; }

        [XmlRpcMember("offset")]
        public int Offset { get; set; }
    }
}

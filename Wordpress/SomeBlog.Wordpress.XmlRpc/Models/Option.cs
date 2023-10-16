using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookComputing.XmlRpc;

namespace SomeBlog.Wordpress.XmlRpc.Models
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class Option
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}

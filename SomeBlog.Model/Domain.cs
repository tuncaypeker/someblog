using System;

namespace SomeBlog.Model
{
    public class Domain : Core.ModelBase
    {
        public string Name { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string Nameserver1 { get; set; }
        public string Nameserver2 { get; set; }
        public string Ip1 { get; set; }
        public string Ip2 { get; set; }

        public int RegistrarId { get; set; }
        public int RegistrarAccountId { get; set; }
        public int DomainWhoisId { get; set; }
    }
}

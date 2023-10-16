using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SomeBlog.Integration.Whois.JsonWhois.Dto
{
    public class WhoisDto
    {
        public string disclaimer { get; set; }
        public string domain { get; set; }
        public string domain_id { get; set; }
        public string status { get; set; }

        [JsonProperty("available?")]
        public bool Available { get; set; }

        [JsonProperty("registered?")]
        public bool Registered { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
        public DateTime expires_on { get; set; }
        public Registrar registrar { get; set; }
        public List<RegistrantContact> registrant_contacts { get; set; }
        public List<AdminContact> admin_contacts { get; set; }
        public List<TechnicalContact> technical_contacts { get; set; }
        public List<Nameserver> nameservers { get; set; }
        public string raw { get; set; }
    }

    public class AdminContact
    {
        public object id { get; set; }
        public int type { get; set; }
        public string name { get; set; }
        public string organization { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
        public object country { get; set; }
        public string country_code { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public object url { get; set; }
        public object created_on { get; set; }
        public object updated_on { get; set; }
    }

    public class Nameserver
    {
        public string name { get; set; }
        public object ipv4 { get; set; }
        public object ipv6 { get; set; }
    }

    public class RegistrantContact
    {
        public object id { get; set; }
        public int type { get; set; }
        public string name { get; set; }
        public string organization { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
        public object country { get; set; }
        public string country_code { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public object url { get; set; }
        public object created_on { get; set; }
        public object updated_on { get; set; }
    }

    public class Registrar
    {
        public string id { get; set; }
        public string name { get; set; }
        public object organization { get; set; }
        public string url { get; set; }
    }

   
    public class TechnicalContact
    {
        public object id { get; set; }
        public int type { get; set; }
        public string name { get; set; }
        public string organization { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
        public object country { get; set; }
        public string country_code { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public object url { get; set; }
        public object created_on { get; set; }
        public object updated_on { get; set; }
    }


}

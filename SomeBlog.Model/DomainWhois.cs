using System;

namespace SomeBlog.Model
{
    public class DomainWhois : Core.ModelBase
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Phone { get; set; }
    }
}

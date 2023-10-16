using System;

namespace SomeBlog.Model
{
    public class PhoneNumber : Core.ModelBase
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

namespace SomeBlog.Model
{
    using System;

    public class ContactMessage : Core.ModelBase
    {
        public int BlogId { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreateDate { get; set; }
        public string Message { get; set; }
        public string Note { get; set; }

        public bool HasRead { get; set; }
    }
}

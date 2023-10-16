namespace SomeBlog.Audit.Dto
{
    public class AuditResultItem
    {
        public AuditResultItem(string category)
        {
            Category = category;
            Message = "";
            HasPassed = false;
        }

        public AuditResultItem(string category,string name)
        {
            Category = category;
            HasPassed = false;
            Name = name;
            Message = "";
        }

        public AuditResultItem(string category, string name, bool hasPassed)
        {
            Category = category;
            HasPassed = hasPassed;
            Name = name;
            Message = "";
        }

        public AuditResultItem(string category, string name, bool hasPassed, string message)
        {
            Category = category;
            HasPassed = hasPassed;
            Name = name;
            Message = message;
        }

        public string Name { get; set; }
        public bool HasPassed { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
    }
}

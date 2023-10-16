namespace SomeBlog.Model
{
    public class JobLog : Core.ModelBase
    {
        public JobLog()
        {
            Description = "";
            Executed = new System.DateTime(1970, 1, 1);
        }

        public int BlogId { get; set; }
        public string Name { get; set; }
        public System.DateTime Execution { get; set; }
        public System.DateTime Executed { get; set; }
        public string Guid { get; set; }
        public bool IsSucceed { get; set; }
        public string Description { get; set; }
    }
}

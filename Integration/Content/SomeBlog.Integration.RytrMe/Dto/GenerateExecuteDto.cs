namespace SomeBlog.Integration.RytrMe.Dto
{
    
    public class GenerateExecuteDataDto
    {
        public string _id { get; set; }
        public string driveIdFileNew { get; set; }
        public string content { get; set; }
        public string contentSingle { get; set; }
    }

    public class GenerateExecuteDto
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public GenerateExecuteDataDto data { get; set; }
    }

}

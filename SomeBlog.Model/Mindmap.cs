namespace SomeBlog.Model
{
    public class Mindmap : Core.ModelBase
    {
        public string Name { get; set; }
        public string MapJson { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Updated { get; set; }
        public string Description { get; set; }
    }
}

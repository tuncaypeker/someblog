using System;

namespace SomeBlog.Model
{
    public class Version : Core.ModelBase
    {
        public string Name { get; set; }
        public string FileSize { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

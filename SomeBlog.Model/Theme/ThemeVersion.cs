using System;

namespace SomeBlog.Model
{
    public class ThemeVersion : Core.ModelBase
    {
        public int ThemeId { get; set; }
        public string Name { get; set; }
        public string FileSize { get; set; }
        public string Description { get; set; }
        public string Hash { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

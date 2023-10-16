namespace SomeBlog.Model
{
    using System;

    public class AlexaSite : Core.ModelBase
    {
        public string Path { get; set; }
        public int CountryRank { get; set; }
        public int WorldRank { get; set; }
        public string Country { get; set; }
        public DateTime LastCheck { get; set; }
    }
}

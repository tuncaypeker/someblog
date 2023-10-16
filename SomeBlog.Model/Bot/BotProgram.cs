namespace SomeBlog.Model
{
    using System;

    /// <summary>
    /// sagda solda calisan bot programlarının aktif calisip calismadigi
    /// ile ilgili tutulan basit bi model
    /// </summary>
    public class BotProgram : Core.ModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastPulse { get; set; }
    }
}

using System;

namespace SomeBlog.Model
{
    /// <summary>
    /// Content edit ekranında Keyword Hitter ile gelen bilgiler, istenilrse loglanır
    /// </summary>
    public class ContentKeywordTail : Core.ModelBase
    {
        public int ContentId { get; set; }
        public string Keyword { get; set; }
        public string Value { get; set; }
    }
}

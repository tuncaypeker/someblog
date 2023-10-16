using System;

namespace SomeBlog.Model
{
    public class BotHistoryLog : Core.ModelBase
    {
        public int BotId { get; set; }
        public int BotHistoryId { get; set; }
        public DateTime Date { get; set; }
        public bool IsSucceed { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// BotId,
        /// TagSayısı
        /// CategorySayısı vs gibi ek bilgi vermek gerekirse buraya yaz
        /// </summary>
        public string Detail { get; set; }
    }
}

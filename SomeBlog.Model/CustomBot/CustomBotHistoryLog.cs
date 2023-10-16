using System;

namespace SomeBlog.Model
{
    public class CustomBotHistoryLog : Core.ModelBase
    {
        public int CustomBotId { get; set; }
        public int CustomBotHistoryId { get; set; }
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

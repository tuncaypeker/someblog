using System;
using TelegramBot = Telegram.Bot;

namespace SomeBlog.Integration.Telegram
{
    public class Service
    {
        private readonly string _token;

        public Service(string token = "5390153090:AAGbFkgDENFKE-LESgjhOMXe9q_ADAdUTgI")
        {
            this._token = token;
        }

        /// <summary>
        /// tuncaypeker
        /// </summary>
        /// <param name="message"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public bool SendMessageAsync(string message, string chatId = "1246177031")
        {
            var telegramBot = new TelegramBot.TelegramBotClient(_token, new System.Net.Http.HttpClient());

            //tuncay
            var me = telegramBot.SendTextMessageAsync(chatId, message).Result;

            return true;
        }
    }
}

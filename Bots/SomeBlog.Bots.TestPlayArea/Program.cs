using System;
using OpenAI_API;

namespace SomeBlog.Bots.TestPlayArea
{
    class Program
    {
        static void Main(string[] args)
        {
            var openAI = new OpenAIAPI(new APIAuthentication("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"));
            var conversation = openAI.Chat.CreateConversation();

            conversation.AppendUserInput("Zafer Algöz ile ilgili bana bir makale yaz, en az 500 kelime olsun.");
            var response = conversation.GetResponseFromChatbotAsync().Result;

            Console.WriteLine(response);
            Console.ReadLine();
        }
    }
}

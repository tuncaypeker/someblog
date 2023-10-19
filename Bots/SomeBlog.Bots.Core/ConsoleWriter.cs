using System;

namespace SomeBlog.Bots.Core
{
    public static class ConsoleWriter
    {
        private static object _MessageLock = new object();

        public static void WriteError(string message)
        {
            lock (_MessageLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public static void WriteSuccess(string message)
        {
            lock (_MessageLock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
 
    }
}

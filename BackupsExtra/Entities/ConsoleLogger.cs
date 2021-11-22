using System;

namespace BackupsExtra.Entities
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.Write(message);
        }
    }
}

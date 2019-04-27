using System;

namespace Sample.TimedConsoleApp.Services
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
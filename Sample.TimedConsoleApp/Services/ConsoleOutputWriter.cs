namespace Sample.TimedConsoleApp.Services
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void Write(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}
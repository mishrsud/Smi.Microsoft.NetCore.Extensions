namespace Sample.TimedConsoleApp.Services
{
    public interface IOutputWriter
    {
        /// <summary> Writes the <paramref name="message"/> to output </summary>
        /// <param name="message">The message to be written to output</param>
        void Write(string message);
    }
}
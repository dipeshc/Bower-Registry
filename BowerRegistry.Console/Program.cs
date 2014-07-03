using ManyConsole;

namespace BowerRegistry.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConsoleCommandDispatcher.DispatchCommand(ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program)), args, System.Console.Out);
        }
    }
}
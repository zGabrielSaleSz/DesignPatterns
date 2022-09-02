namespace Super.DesignPattern
{
    public class AbstractFactoryExample : IDesignPatternExample
    {
        public void Run()
        {
            IConsoleFactory consoleFactory = new BlueThemeConsole();
            StartApplication(consoleFactory);
        }

        private void StartApplication(IConsoleFactory consoleFactory)
        {
            ConsoleWriter consoleWriter = consoleFactory.CreateConsoleWriter();
            consoleWriter.WriteLine("Hello world!");
        }
    }

    public interface IConsoleFactory
    {
        ConsoleWriter CreateConsoleWriter();
    }

    public class LightThemeConsole : IConsoleFactory
    {
        public ConsoleWriter CreateConsoleWriter()
        {
            return new ConsoleWriter(ConsoleColor.Black, ConsoleColor.White);
        }
    }

    public class DarkThemeConsole : IConsoleFactory
    {
        public ConsoleWriter CreateConsoleWriter()
        {
            return new ConsoleWriter(ConsoleColor.White, ConsoleColor.Black);
        }
    }

    public class BlueThemeConsole : IConsoleFactory
    {
        public ConsoleWriter CreateConsoleWriter()
        {
            return new ConsoleWriter(ConsoleColor.Cyan, ConsoleColor.Black);
        }
    }

    public class ConsoleWriter
    {
        public static ConsoleColor defaultForegroundColor = Console.ForegroundColor;
        public static ConsoleColor defaultBackgroundColor = Console.BackgroundColor;

        public ConsoleColor _foregroundColor = Console.ForegroundColor;
        public ConsoleColor _backgroundColor = Console.BackgroundColor;

        public ConsoleWriter(ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            if (foregroundColor.HasValue)
                _foregroundColor = foregroundColor.Value;

            if (backgroundColor.HasValue)
                _backgroundColor = backgroundColor.Value;

        }

        public static object lockedResource = new object();
        public void WriteLine(string text)
        {
            lock (lockedResource)
            {

                Console.ForegroundColor = _foregroundColor;
                Console.BackgroundColor = _backgroundColor;
                Console.WriteLine(text);
                Console.ForegroundColor = defaultForegroundColor;
                Console.BackgroundColor = defaultBackgroundColor;
            }
        }
    }
}

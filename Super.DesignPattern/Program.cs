using Super.DesignPattern;

internal class Program
{
    private static void Main(string[] args)
    {
        // Switch the class as you wanna debug
        IDesignPatternExample example = new AbstractFactoryExample();
        example.Run();
    }
}
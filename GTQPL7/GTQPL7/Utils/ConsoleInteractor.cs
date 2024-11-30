namespace GTQPL7.Utils;

public class ConsoleInteractor : IInteractor
{
    public string? GetInput(string? message = null)
    {
        if (message != null)
        {
            Console.WriteLine(message);
        }
        return Console.ReadLine();
    }
}
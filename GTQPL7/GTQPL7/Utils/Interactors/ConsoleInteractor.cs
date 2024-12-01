namespace GTQPL7.Utils.Interactors;

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
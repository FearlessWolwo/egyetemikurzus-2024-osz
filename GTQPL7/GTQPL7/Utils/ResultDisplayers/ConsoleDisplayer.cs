namespace GTQPL7.Utils.ResultDisplayers;

public class ConsoleDisplayer : IResultDisplayer
{
    public void DisplayResult(string message)
    {
        Console.WriteLine(message);
    }
}
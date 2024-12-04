namespace GTQPL7.Utils.ResultDisplayers;

public class FileDisplayer : IResultDisplayer
{
    private string _fileName;

    public FileDisplayer(string fileName)
    {
        _fileName = fileName;
    }
    
    public void DisplayResult(string message)
    {
        File.WriteAllText(_fileName, message);
        Console.WriteLine($"Output written to {_fileName}.");
    }
}
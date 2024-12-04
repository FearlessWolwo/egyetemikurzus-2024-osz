namespace GTQPL7.Utils.Interactors;

public class FileInteractor : IInteractor
{
    private string[] _lines;
    private int _lineIndex;

    public FileInteractor(string[] fileContents)
    {
        _lines = fileContents;
        _lineIndex = 1;
    }
    
    public string? GetInput(string? message = null)
    {
        return _lines[_lineIndex++];
    }
}
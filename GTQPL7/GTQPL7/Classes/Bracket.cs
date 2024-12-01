namespace GTQPL7.Classes;

public class Bracket : Operator
{
    public Bracket(string identifier) : base(identifier, 0)
    {
        IsLeftBracket = identifier == "(";
    }
    
    public bool IsLeftBracket { get; }
}
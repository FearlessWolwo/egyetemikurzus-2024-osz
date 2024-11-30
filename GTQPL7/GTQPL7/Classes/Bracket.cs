namespace GTQPL7.Classes;

public class Bracket : MathSymbol
{
    public Bracket(string identifier) : base(identifier)
    {
        IsLeftBracket = identifier == "(";
    }
    
    public bool IsLeftBracket { get; }
}
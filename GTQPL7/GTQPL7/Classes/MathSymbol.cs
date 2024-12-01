namespace GTQPL7.Classes;

public abstract class MathSymbol
{
    protected MathSymbol(string identifier)
    {
        Identifier = identifier;
    }
    
    public string Identifier { get; }

    public override string ToString()
    {
        return Identifier;
    }
}
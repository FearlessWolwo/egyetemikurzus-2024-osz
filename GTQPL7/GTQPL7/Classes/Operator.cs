namespace GTQPL7.Classes;

public abstract class Operator : IMathSymbol
{
    protected Operator(int precedence)
    {
        Precedence = precedence;
    }
    
    public int Precedence { get; }
}
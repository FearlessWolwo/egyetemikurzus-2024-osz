namespace GTQPL7.Classes;

public class Operator : MathSymbol
{
    public Operator(string identifier, int precedence) : base(identifier)
    {
        Precedence = precedence;
    }
    
    public int Precedence { get; }
}
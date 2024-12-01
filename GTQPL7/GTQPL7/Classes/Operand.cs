using System.Numerics;

namespace GTQPL7.Classes;

public class Operand : MathSymbol
{
    public Operand(string identifier) : base(identifier) { }

    public Operand(string identifier, double value) : base(identifier)
    {
        Value = value;
    }

    public double? Value { get; set; }
}
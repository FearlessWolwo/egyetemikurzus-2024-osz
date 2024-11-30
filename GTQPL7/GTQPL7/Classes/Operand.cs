using System.Numerics;

namespace GTQPL7.Classes;

public class Operand<T> : MathSymbol where T : ISignedNumber<T>
{
    public Operand(string identifier) : base(identifier) { }

    public Operand(string identifier, T value) : base(identifier)
    {
        Value = value;
    }

    public T? Value { get; set; }
}
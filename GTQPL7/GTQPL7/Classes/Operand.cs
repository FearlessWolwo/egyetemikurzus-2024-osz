using System.Numerics;

namespace GTQPL7.Classes;

public abstract class Operand<T> : IMathSymbol where T : INumber<T>
{
    protected Operand(T value)
    {
        Value = value;
    }
    
    protected Operand() {}
    public T? Value { get; }
}
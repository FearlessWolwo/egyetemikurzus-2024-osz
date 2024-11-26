using System.Numerics;

namespace GTQPL7.Classes;

public class Scalar<T> : Operand<T> where T : INumber<T>
{
    public Scalar(T value) : base(value) {}
}
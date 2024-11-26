using System.Numerics;

namespace GTQPL7.Classes;

public class ScalarParameter<T> : Operand<T> where T : INumber<T>
{
    public ScalarParameter(T value) : base(value) {}

    public ScalarParameter() {}
}
using System.Numerics;

namespace GTQPL7.Classes;

public class MatrixOperand<T> : MathSymbol where T : ISignedNumber<T>
{
    public MatrixOperand(string identifier) : base(identifier) { }

    public MatrixOperand(string identifier, Matrix<T> value) : base(identifier)
    {
        Value = value;
    }

    public Matrix<T>? Value { get; set; }
}
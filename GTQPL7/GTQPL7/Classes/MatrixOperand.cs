using System.Numerics;

namespace GTQPL7.Classes;

public class MatrixOperand : MathSymbol
{
    public MatrixOperand(string identifier) : base(identifier) { }

    public MatrixOperand(string identifier, Matrix value) : base(identifier)
    {
        Value = value;
    }

    public Matrix? Value { get; set; }
}
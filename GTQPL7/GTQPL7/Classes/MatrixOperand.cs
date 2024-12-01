namespace GTQPL7.Classes;

public class MatrixOperand : MathSymbol, IOperand
{
    public MatrixOperand(string identifier) : base(identifier) { }

    public MatrixOperand(string identifier, Matrix value) : base(identifier)
    {
        Value = value;
    }

    public Matrix Value { get; set; }
    
    public string GetValueAsString() { return Value.ToString(); }
}
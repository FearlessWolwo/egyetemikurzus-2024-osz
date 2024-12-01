using System.Globalization;
using System.Numerics;

using GTQPL7.Classes;

namespace GTQPL7.Utils;

public class MatrixValueAssigner
{
    public MatrixValueAssigner(IInteractor interactor)
    {
        Interactor = interactor;
    }
    
    public IInteractor Interactor { get; set; }
    
    public void AssignValue(MatrixOperand matrixOperand)
    {
        List<double> values = new List<double>();

        string? line = Interactor.GetInput($"Please enter the dimensions of {matrixOperand.Identifier}");
        string[] dimensions = line!.Split(" ");
        int rows = int.Parse(dimensions[0]);
        int columns = int.Parse(dimensions[1]);

        for (int i = 0; i < rows; i++)
        {
            line = Interactor.GetInput();
            string[] stringValues = line!.Split(" ");
            values.AddRange(stringValues.Select(stringValue => Double.Parse(stringValue.AsSpan(), new NumberFormatInfo() { NumberDecimalSeparator = "." })));
        }
        
        matrixOperand.Value = new Matrix(rows, columns, values.ToArray());
    }
}
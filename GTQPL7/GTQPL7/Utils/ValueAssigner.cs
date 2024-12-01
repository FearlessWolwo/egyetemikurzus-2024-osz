using System.Globalization;
using System.Numerics;

using GTQPL7.Classes;

namespace GTQPL7.Utils;

public class ValueAssigner
{
    public ValueAssigner(IInteractor interactor)
    {
        Interactor = interactor;
    }
    
    public IInteractor Interactor { get; set; }
    
    public void AssignValue(Operand operand)
    {
        string? line = Interactor.GetInput($"Please define {operand.Identifier}:");
        if (line != null)
        {
            operand.Value = Double.Parse(line.AsSpan(), new NumberFormatInfo() { NumberDecimalSeparator = "." });
        }
    }
}
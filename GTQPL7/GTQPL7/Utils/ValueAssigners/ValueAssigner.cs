using System.Globalization;

using GTQPL7.Classes;
using GTQPL7.Utils.Interactors;

namespace GTQPL7.Utils.ValueAssigners;

public class ValueAssigner : IValueAssigner<Operand>
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
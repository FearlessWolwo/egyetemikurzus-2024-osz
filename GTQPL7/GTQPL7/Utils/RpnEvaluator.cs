using GTQPL7.Classes;
using GTQPL7.Utils.ResultDisplayers;

namespace GTQPL7.Utils;

public class RpnEvaluator
{
    private Stack<MathSymbol> _operandStack;
    private IResultDisplayer _resultDisplayer;

    public RpnEvaluator() : this(new ConsoleDisplayer()) { }
    
    public RpnEvaluator(IResultDisplayer resultDisplayer)
    {
        _operandStack = new Stack<MathSymbol>();
        _resultDisplayer = resultDisplayer;
    }
    
    public IResultDisplayer ResultDisplayer => _resultDisplayer;

    public void Evaluate(Queue<MathSymbol> reversePolishNotation)
    {
        while (reversePolishNotation.Count > 0)
        {
            if (reversePolishNotation.Peek() is IOperand)
            {
                _operandStack.Push(reversePolishNotation.Dequeue());
                continue;
            }

            MathSymbol rhs;
            MathSymbol lhs;
            switch (reversePolishNotation.Peek().Identifier)
            {
                case "+":
                    rhs = _operandStack.Pop();
                    lhs = _operandStack.Pop();
                    switch (lhs)
                    {
                        case Operand lhsOperand when rhs is Operand rhsOperand:
                            _operandStack.Push(new Operand("x", lhsOperand.Value + rhsOperand.Value));
                            break;
                        case MatrixOperand lhsMatrixOperand when rhs is MatrixOperand rhsMatrixOperand:
                            _operandStack.Push(new MatrixOperand("X", lhsMatrixOperand.Value + rhsMatrixOperand.Value));
                            break;
                        default:
                            throw new ArithmeticException($"Could not add {lhs} to {rhs}");
                    }
                    break;
                case "-":
                    rhs = _operandStack.Pop();
                    lhs = _operandStack.Pop();
                    switch (lhs)
                    {
                        case Operand lhsOperand when rhs is Operand rhsOperand:
                            _operandStack.Push(new Operand("x", lhsOperand.Value - rhsOperand.Value));
                            break;
                        case MatrixOperand lhsMatrixOperand when rhs is MatrixOperand rhsMatrixOperand:
                            _operandStack.Push(new MatrixOperand("X", lhsMatrixOperand.Value - rhsMatrixOperand.Value));
                            break;
                        default:
                            throw new ArithmeticException($"Could not subtract {lhs} from {rhs}");
                    }
                    break;
                case "*":
                    rhs = _operandStack.Pop();
                    lhs = _operandStack.Pop();
                    switch (lhs)
                    {
                        case Operand lhsOperand when rhs is MatrixOperand rhsOperand:
                            _operandStack.Push(new MatrixOperand("X", rhsOperand.Value.MultiplyByScalar(lhsOperand.Value)));
                            break;
                        case MatrixOperand lhsMatrixOperand when rhs is MatrixOperand rhsMatrixOperand:
                            _operandStack.Push(new MatrixOperand("X", lhsMatrixOperand.Value * rhsMatrixOperand.Value));
                            break;
                        default:
                            throw new ArithmeticException($"Could not multiply {lhs} by {rhs}");
                    }
                    break;
                case "inv":
                    lhs = _operandStack.Pop();
                    switch (lhs)
                    {
                        case MatrixOperand lhsMatrixOperand:
                            _operandStack.Push(new MatrixOperand("X", lhsMatrixOperand.Value.Invert()));
                            break;
                        default:
                            throw new ArithmeticException($"Could not invert {lhs}");
                    }
                    break;
                case "trans":
                    lhs = _operandStack.Pop();
                    switch (lhs)
                    {
                        case MatrixOperand lhsMatrixOperand:
                            _operandStack.Push(new MatrixOperand("X", lhsMatrixOperand.Value.Transpose()));
                            break;
                        default:
                            throw new ArithmeticException($"Could not transpose {lhs}");
                    }

                    break;
                case "det":
                    lhs = _operandStack.Pop();
                    switch (lhs)
                    {
                        case MatrixOperand lhsMatrixOperand:
                            _operandStack.Push(new Operand("x", lhsMatrixOperand.Value.Determinant()));
                            break;
                        default:
                            throw new ArithmeticException($"Could not calculate determinant of {lhs}");
                    }
                    break;
                default:
                    throw new ArgumentException($"Unknown operator: {reversePolishNotation.Peek().Identifier}");
            }

            reversePolishNotation.Dequeue();
        }

        if (_operandStack.Count != 1 || _operandStack.Peek() is not IOperand operand)
        {
            throw new ArithmeticException($"The given expression could not be evaluated");
        }
        _resultDisplayer.DisplayResult("Result is:" + System.Environment.NewLine + operand.GetValueAsString());
        _operandStack.Clear();
    }
}
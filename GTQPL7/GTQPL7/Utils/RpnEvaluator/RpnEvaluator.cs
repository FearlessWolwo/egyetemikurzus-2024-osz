using GTQPL7.Classes;
using GTQPL7.Exceptions;
using GTQPL7.Utils.ResultDisplayers;

namespace GTQPL7.Utils.RpnEvaluator;

public class RpnEvaluator : IRpnEvaluator
{
    private Stack<MathSymbol> _operandStack;
    private IResultDisplayer _resultDisplayer;

    public RpnEvaluator() : this(new ConsoleDisplayer()) { }
    
    public RpnEvaluator(IResultDisplayer resultDisplayer)
    {
        _operandStack = new Stack<MathSymbol>();
        _resultDisplayer = resultDisplayer;
    }
    
    public IResultDisplayer ResultDisplayer
    {
        get => _resultDisplayer;
        set => _resultDisplayer = value;
    }

    public void Evaluate(Queue<MathSymbol> reversePolishNotation)
    {
        _operandStack.Clear();
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
                    if (_operandStack.Count < 2)
                    {
                        throw new RpnEvaluatorException("Too few operands for addition");
                    }
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
                            throw new RpnEvaluatorException($"Could not add {lhs.GetType()} to {rhs.GetType()}");
                    }
                    break;
                case "-":
                    if (_operandStack.Count < 2)
                    {
                        throw new RpnEvaluatorException("Too few operands for subtraction");
                    }
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
                            throw new RpnEvaluatorException($"Could not subtract {lhs.GetType()} from {rhs.GetType()}");
                    }
                    break;
                case "*":
                    if (_operandStack.Count < 2)
                    {
                        throw new RpnEvaluatorException("Too few operands for multiplication");
                    }
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
                            throw new RpnEvaluatorException($"Could not multiply {rhs.GetType()} by {lhs.GetType()}");
                    }
                    break;
                case "inv":
                    if (_operandStack.Count < 1)
                    {
                        throw new RpnEvaluatorException("Too few operands for inversion");
                    }
                    lhs = _operandStack.Pop();
                    switch (lhs)
                    {
                        case MatrixOperand lhsMatrixOperand:
                            _operandStack.Push(new MatrixOperand("X", lhsMatrixOperand.Value.Invert()));
                            break;
                        default:
                            throw new RpnEvaluatorException($"Could not invert {lhs.GetType()}");
                    }
                    break;
                case "trans":
                    if (_operandStack.Count < 1)
                    {
                        throw new RpnEvaluatorException("Too few operands for transposition");
                    }
                    lhs = _operandStack.Pop();
                    switch (lhs)
                    {
                        case MatrixOperand lhsMatrixOperand:
                            _operandStack.Push(new MatrixOperand("X", lhsMatrixOperand.Value.Transpose()));
                            break;
                        default:
                            throw new RpnEvaluatorException($"Could not transpose {lhs.GetType()}");
                    }

                    break;
                case "det":
                    if (_operandStack.Count < 1)
                    {
                        throw new RpnEvaluatorException("Too few operands for calculating determinant");
                    }
                    lhs = _operandStack.Pop();
                    switch (lhs)
                    {
                        case MatrixOperand lhsMatrixOperand:
                            _operandStack.Push(new Operand("x", lhsMatrixOperand.Value.Determinant()));
                            break;
                        default:
                            throw new RpnEvaluatorException($"Could not calculate determinant of {lhs.GetType()}");
                    }
                    break;
                default:
                    throw new RpnEvaluatorException($"Unknown operator: {reversePolishNotation.Peek().GetType()}");
            }

            reversePolishNotation.Dequeue();
        }

        if (_operandStack.Count != 1 || _operandStack.Peek() is not IOperand operand)
        {
            throw new RpnEvaluatorException("The given expression could not be evaluated");
        }
        _resultDisplayer.DisplayResult("Result is:" + System.Environment.NewLine + operand.GetValueAsString());
    }
}
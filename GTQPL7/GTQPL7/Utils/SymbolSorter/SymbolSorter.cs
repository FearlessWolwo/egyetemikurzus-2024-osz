using GTQPL7.Classes;
using GTQPL7.Exceptions;

namespace GTQPL7.Utils.SymbolSorter;

public class SymbolSorter : ISymbolSorter
{
    private const string LeftBracketError = "Mismatched left bracket found";
    private const string RightBracketError = "Mismatched right bracket found";
    public Queue<MathSymbol> Sort(List<MathSymbol> symbols)
    {
        Queue<MathSymbol> outputQueue = new Queue<MathSymbol>();
        Stack<Operator> operatorStack = new Stack<Operator>();
        foreach (MathSymbol symbol in symbols)
        {
            switch (symbol)
            {
                case Bracket { IsLeftBracket: true } bracket:
                    operatorStack.Push(bracket);
                    break;
                case Bracket bracket:
                    {
                        while (operatorStack.Count != 0 && operatorStack.Peek() is Bracket == false)
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }

                        if (operatorStack.Count == 0)
                        {
                            throw new SymbolSorterException(RightBracketError);
                        }
                        operatorStack.Pop();
                        break;
                    }
                case Operator op:
                    {
                        while (operatorStack.Count != 0 && operatorStack.Peek().Precedence > op.Precedence)
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        operatorStack.Push(op);
                        break;
                    }
                default:
                    outputQueue.Enqueue(symbol);
                    break;
            }
        }

        foreach (Operator op in operatorStack)
        {
            if (op is Bracket)
            {
                throw new SymbolSorterException(LeftBracketError);
            }
            outputQueue.Enqueue(op);
        }

        return outputQueue;
    }
}
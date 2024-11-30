using System.Globalization;
using System.Numerics;

using GTQPL7.Classes;
using GTQPL7.Utils.Tokenizer;

namespace GTQPL7.Utils.TokenConverter;

public class TokenConverter<T> : ITokenConverter where T : ISignedNumber<T>
{
    private IInteractor _interactor;
    private readonly ValueAssigner<T> _valueAssigner;
    private readonly MatrixValueAssigner<T> _matrixValueAssigner;
    private readonly Dictionary<string, int> _binaryOperatorPrecedences = new Dictionary<string, int>()
    {
        { "+", 1 },
        { "-", 1 },
        { "*", 2 }
    };

    public TokenConverter() : this(new ConsoleInteractor()) { }
    
    private TokenConverter(IInteractor interactor)
    {
        _interactor = interactor;
        _valueAssigner = new ValueAssigner<T>(interactor);
        _matrixValueAssigner = new MatrixValueAssigner<T>(interactor);
    }

    public IInteractor Interactor
    {
        get => _interactor;
        set
        {
            _interactor = value;
            _valueAssigner.Interactor = value;
            _matrixValueAssigner.Interactor = value;
        }
    }
    
    public List<MathSymbol> ConvertDslTokensToMathSymbols(List<DslToken> tokens)
    {
        List<MathSymbol> mathSymbols = new List<MathSymbol>();
        foreach (DslToken token in tokens)
        {
            switch (token.TokenType)
            {
                case TokenType.ClosingBracket:
                case TokenType.OpeningBracket:
                    mathSymbols.Add(new Bracket(token.Value));
                    break;
                case TokenType.BinaryOperator:
                    mathSymbols.Add(new Operator(token.Value, _binaryOperatorPrecedences[token.Value]));
                    break;
                case TokenType.UnaryOperator:
                    mathSymbols.Add(new Operator(token.Value, 3));
                    break;
                case TokenType.IntegerValue:
                    mathSymbols.Add(new Operand<int>(token.Value, int.Parse(token.Value)));
                    break;
                case TokenType.RealValue:
                    mathSymbols.Add(new Operand<double>(token.Value, Double.Parse(token.Value)));
                    break;
                case TokenType.ComplexValue:
                    mathSymbols.Add(new Operand<Complex>(token.Value,
                        Complex.Parse(token.Value, new NumberFormatInfo() { NumberDecimalSeparator = "." })));
                    break;
                case TokenType.Parameter:
                    Operand<T> operand = new Operand<T>(token.Value);
                    _valueAssigner.AssignValue(operand);
                    mathSymbols.Add(operand);
                    break;
                case TokenType.Matrix:
                    MatrixOperand<T> matrix = new MatrixOperand<T>(token.Value);
                    _matrixValueAssigner.AssignValue(matrix);
                    mathSymbols.Add(matrix);
                    break;
                default:
                    throw new Exception($"Unexpected token type {token.TokenType}");
            }
        }
        return mathSymbols;
    }
}
using System.Numerics;

using DeepEqual.Syntax;

using GTQPL7.Classes;
using GTQPL7.Utils;
using GTQPL7.Utils.TokenConverter;
using GTQPL7.Utils.Tokenizer;

using NSubstitute;

namespace GTQPL7_Tests.Utils.TokenConverter;

[TestFixture]
public class TokenConverterTests
{
    private ITokenConverter _tokenConverter;

    [Test]
    public void ConvertDslTokensToMathSymbols_ValidComplexTokens_ShouldReturnMathSymbols()
    {
        List<MathSymbol> expectedSymbols =
        [
            new Operand<Complex>("a", new Complex(1, 0)),
            new Operator("*", 2),
            new MatrixOperand<Complex>("A", new Matrix<Complex>(2, 2, [
                new Complex(1, 0),
                new Complex(0, 2),
                new Complex(3, 0.5),
                new Complex(4, -1)
            ]))
        ];
        List<DslToken> tokens =
        [
            new DslToken(TokenType.Parameter, "a"),
            new DslToken(TokenType.BinaryOperator, "*"),
            new DslToken(TokenType.Matrix, "A")
        ];
        IInteractor interactor = Substitute.For<IInteractor>();
        interactor.GetInput(Arg.Any<string?>())
            .Returns("<1;0>", "2 2", "<1;0> <0;2>", "<3;0.5> <4;-1>");
        _tokenConverter = new TokenConverter<Complex> { Interactor = interactor };

        List<MathSymbol> symbols = _tokenConverter.ConvertDslTokensToMathSymbols(tokens);
        
        Assert.That(symbols.IsDeepEqual(expectedSymbols));
    }

    [Test]
    public void ConvertDslTokensToMathSymbols_ValidRealTokens_ShouldReturnMathSymbols()
    {
        List<MathSymbol> expectedSymbols =
        [
            new Operand<Double>("a", 0.2),
            new Operator("+", 1),
            new Operator("inv", 3),
            new MatrixOperand<Double>("A", new Matrix<Double>(1, 3, [
                3.3,
                2.2,
                1.1
            ]))
        ];
        List<DslToken> tokens =
        [
            new DslToken(TokenType.Parameter, "a"),
            new DslToken(TokenType.BinaryOperator, "+"),
            new DslToken(TokenType.UnaryOperator, "inv"),
            new DslToken(TokenType.Matrix, "A")
        ];
        IInteractor interactor = Substitute.For<IInteractor>();
        interactor.GetInput(Arg.Any<string?>())
            .Returns("0.2", "1 3", "3.3 2.2 1.1");
        _tokenConverter = new TokenConverter<Double> { Interactor = interactor };
        
        List<MathSymbol> symbols = _tokenConverter.ConvertDslTokensToMathSymbols(tokens);
        
        Assert.That(symbols.IsDeepEqual(expectedSymbols));
    }
}
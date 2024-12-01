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
    public void ConvertDslTokensToMathSymbols_ValidTokens_ShouldReturnMathSymbols()
    {
        List<MathSymbol> expectedSymbols =
        [
            new Operand("a", 1.0),
            new Operator("*", 2),
            new MatrixOperand("A", new Matrix(2, 2, [
                1.0, 0.2,
                3.5, -4.1
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
            .Returns("1.0", "2 2", "1.0 0.2", "3.5 -4.1");
        _tokenConverter = new GTQPL7.Utils.TokenConverter.TokenConverter() { Interactor = interactor };

        List<MathSymbol> symbols = _tokenConverter.ConvertDslTokensToMathSymbols(tokens);
        
        Assert.That(symbols.IsDeepEqual(expectedSymbols));
    }
}
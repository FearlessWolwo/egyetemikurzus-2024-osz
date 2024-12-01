using DeepEqual.Syntax;

namespace GTQPL7_Tests.Utils.Tokenizer;
using GTQPL7.Utils.Tokenizer;

[TestFixture]
public class TokenizerTests
{
    private Tokenizer _tokenizer;

    [OneTimeSetUp]
    public void SetUp()
    {
        _tokenizer = new Tokenizer();
    }

    [Test]
    public void Tokenize_ValidTokens_ShouldReturnTokens()
    {
        const string input = "(2A + B) * inv(C)";
        List<DslToken> expectedTokens =
        [
            new(TokenType.OpeningBracket, "("),
            new(TokenType.Value, "2"),
            new(TokenType.Matrix, "A"),
            new(TokenType.BinaryOperator, "+"),
            new(TokenType.Matrix, "B"),
            new(TokenType.ClosingBracket, ")"),
            new(TokenType.BinaryOperator, "*"),
            new(TokenType.UnaryOperator, "inv"),
            new(TokenType.OpeningBracket, "("),
            new(TokenType.Matrix, "C"),
            new(TokenType.ClosingBracket, ")")
        ];

        List<DslToken> tokens = _tokenizer.Tokenize(input);
        
        Assert.That(expectedTokens.IsDeepEqual(tokens));
    }

    [Test]
    public void Tokenize_InvalidTokens_ShouldThrowTokenizerException()
    {
        const string input = "det(2A + B) = x";
        
        Assert.That(() => _tokenizer.Tokenize(input), Throws.TypeOf<TokenizerException>()
            .With.Message.EqualTo("\"= x\" could not be tokenized."));
    }
}
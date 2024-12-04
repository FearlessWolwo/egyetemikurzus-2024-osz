using System.Text.RegularExpressions;

namespace GTQPL7.Utils.Tokenizer;

public class Tokenizer : ITokenizer
{
    private const string IntegerRegex = "0|[1-9][0-9]*";
    private const string RealRegex = $"-?({IntegerRegex}|{IntegerRegex}[.][0-9]+)";
    private readonly List<TokenDefinition> _tokensDefinitions =
    [
        new("^\\(", TokenType.OpeningBracket),
        new("^\\)", TokenType.ClosingBracket),
        new("^\\+", TokenType.BinaryOperator),
        new("^-", TokenType.BinaryOperator),
        new("^\\*", TokenType.BinaryOperator),
        new("^inv", TokenType.UnaryOperator),
        new("^trans", TokenType.UnaryOperator),
        new("^det", TokenType.UnaryOperator),
        new($"^{RealRegex}", TokenType.Value),
        new("^[A-Z]", TokenType.Matrix),
        new("^[a-z]", TokenType.Parameter)
    ];

    public List<DslToken> Tokenize(string text)
    {
        List<DslToken> tokens = new List<DslToken>();
        string remainingText = text;

        while (!String.IsNullOrWhiteSpace(remainingText))
        {
            TokenMatch match = FindMatch(remainingText);
            if (match.Matched)
            {
                tokens.Add(new DslToken(match.TokenType, match.Value));
                remainingText = remainingText[match.Value.Length..];
            }
            else
            {
                if (MatchWhiteSpace(remainingText))
                {
                    remainingText = remainingText[1..];
                }
                else
                {
                    throw new TokenizerException($"\"{remainingText}\" could not be tokenized.");
                }
            }
        }

        return tokens;
    }

    private bool MatchWhiteSpace(string text)
    {
        return Regex.IsMatch(text, @"^\s");
    }

    private TokenMatch FindMatch(string text)
    {
        for (int i = 0; i < _tokensDefinitions.Count; i++)
        {
            TokenMatch match = _tokensDefinitions[i].Match(text);
            if (match.Matched)
            {
                return match;
            }
        }

        return new TokenMatch(TokenType.Error, false);
    }
}
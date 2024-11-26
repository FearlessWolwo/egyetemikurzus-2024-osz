using System.Text.RegularExpressions;

namespace GTQPL7.Utils.Tokenizer;

public class Tokenizer
{
    private const string IntegerRegex = "(0|[1-9][0-9]*)";
    private const string RealRegex = $"{IntegerRegex}[.,][0-9]+";
    private const string ComplexRegex = $"{RealRegex}i(?!nv\\()";
    private readonly List<TokenDefinition> _tokensDefinitions =
    [
        new("^\\(", TokenType.Operator),
        new("^\\)", TokenType.Operator),
        new("^\\+", TokenType.Operator),
        new("^-", TokenType.Operator),
        new("^\\*", TokenType.Operator),
        new("^inv", TokenType.Operator),
        new("^trans", TokenType.Operator),
        new("^det", TokenType.Operator),
        new("^htrans", TokenType.Operator),
        new($"^{ComplexRegex}", TokenType.Value),
        new($"^{RealRegex}", TokenType.Value),
        new($"^{IntegerRegex}", TokenType.Value),
        new("^[A-Z]", TokenType.Value),
        new("^[a-z]", TokenType.Value)
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
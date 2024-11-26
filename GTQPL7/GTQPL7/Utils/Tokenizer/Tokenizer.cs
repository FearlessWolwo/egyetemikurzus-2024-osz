using System.Text.RegularExpressions;

namespace GTQPL7.Utils.Tokenizer;

public class Tokenizer
{
    private static readonly string INTEGER_REGEX = "(0|[1-9][0-9]+)";
    private static readonly string REAL_REGEX = $"{INTEGER_REGEX}[.,][0-9]+";
    private static readonly string COMPLEX_REGEX = $"{REAL_REGEX}i(?!nv\\()";
    private List<TokenDefinition> _tokensDefinitions;

    public Tokenizer()
    {
        _tokensDefinitions =
        [
            new TokenDefinition("^\\(", TokenType.Operator),
            new TokenDefinition("^\\)", TokenType.Operator),
            new TokenDefinition("^\\+", TokenType.Operator),
            new TokenDefinition("^-", TokenType.Operator),
            new TokenDefinition("^\\*", TokenType.Operator),
            new TokenDefinition("^inv", TokenType.Operator),
            new TokenDefinition("^trans", TokenType.Operator),
            new TokenDefinition("^det", TokenType.Operator),
            new TokenDefinition("^htrans", TokenType.Operator),
            new TokenDefinition($"^{COMPLEX_REGEX}", TokenType.Value),
            new TokenDefinition($"^{REAL_REGEX}", TokenType.Value),
            new TokenDefinition($"^{INTEGER_REGEX}", TokenType.Value),
            new TokenDefinition("^[A-Z]", TokenType.Value),
            new TokenDefinition("^[a-z]", TokenType.Value)
        ];
    }

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
                    throw new TokenizerException($"{remainingText} couldn't be tokenized.");
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
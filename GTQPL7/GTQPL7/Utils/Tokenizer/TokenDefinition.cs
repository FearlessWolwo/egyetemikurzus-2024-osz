using System.Text.RegularExpressions;

namespace GTQPL7.Utils.Tokenizer;

public class TokenDefinition
{
    public TokenDefinition(string regexString, TokenType tokenType)
    {
        TokenType = tokenType;
        Regex = new Regex(regexString);
    }

    public Regex Regex { get; }
    public TokenType TokenType { get; }

    public TokenMatch Match(string text)
    {
        Match match = Regex.Match(text);
        if (!match.Success)
        {
            return new TokenMatch(TokenType.Error, false);
        }

        string remainingText = text[match.Length..];
        return new TokenMatch(TokenType, match.Value, true, remainingText);
    }
}
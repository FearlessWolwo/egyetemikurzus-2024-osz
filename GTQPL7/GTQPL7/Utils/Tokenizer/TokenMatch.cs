namespace GTQPL7.Utils.Tokenizer;

public class TokenMatch
{
    public TokenMatch(TokenType tokenType, string value, bool matched, string remainingText)
    {
        TokenType = tokenType;
        Value = value;
        Matched = matched;
        RemainingText = remainingText;
    }

    public TokenMatch(TokenType tokenType, bool matched)
    {
        TokenType = tokenType;
        Matched = matched;
    }
    
    public TokenType TokenType { get; }
    public string? Value { get; }
    public bool Matched { get; }
    public string? RemainingText { get; }
}
namespace GTQPL7.Utils.Tokenizer;

public class DslToken
{
    public DslToken(TokenType tokenType, string value)
    {
        TokenType = tokenType;
        Value = value;
    }
    
    public TokenType TokenType { get; }
    public string Value { get; }
}
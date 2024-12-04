namespace GTQPL7.Utils.Tokenizer;

public interface ITokenizer
{
    public List<DslToken> Tokenize(string text);
}
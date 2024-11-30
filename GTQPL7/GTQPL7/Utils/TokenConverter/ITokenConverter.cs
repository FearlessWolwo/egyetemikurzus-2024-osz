using GTQPL7.Classes;
using GTQPL7.Utils.Tokenizer;

namespace GTQPL7.Utils.TokenConverter;

public interface ITokenConverter
{
    List<MathSymbol> ConvertDslTokensToMathSymbols(List<DslToken> tokens);
}
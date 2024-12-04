using GTQPL7.Classes;

namespace GTQPL7.Utils.SymbolSorter;

public interface ISymbolSorter
{
    public Queue<MathSymbol> Sort(List<MathSymbol> symbols);
}
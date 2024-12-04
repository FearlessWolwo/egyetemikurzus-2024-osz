using GTQPL7.Classes;

namespace GTQPL7.Utils.RpnEvaluator;

public interface IRpnEvaluator
{
    public void Evaluate(Queue<MathSymbol> expression);
}
using DeepEqual.Syntax;

using GTQPL7.Classes;
using GTQPL7.Exceptions;
using GTQPL7.Utils;

namespace GTQPL7_Tests.Utils.SymbolSorter;

[TestFixture]
public class SymbolSorterTests
{
    private GTQPL7.Utils.SymbolSorter.SymbolSorter _symbolSorter;

    [OneTimeSetUp]
    public void Setup()
    {
        _symbolSorter = new GTQPL7.Utils.SymbolSorter.SymbolSorter();
    }

    [Test]
    public void SortTokens_ValidBrackets_ShouldReturnSortedTokens()
    {
        List<MathSymbol> symbols =
        [
            new Bracket("("),
            new Operand("a", 5),
            new Operator("+", 1),
            new Operand("b", 10),
            new Bracket(")"),
            new Operator("*", 2),
            new Operand("c", 20)
        ];
        Queue<MathSymbol> expected = new Queue<MathSymbol>();
        expected.Enqueue(new Operand("a", 5));
        expected.Enqueue(new Operand("b", 10));
        expected.Enqueue(new Operator("+", 1));
        expected.Enqueue(new Operand("c", 20));
        expected.Enqueue(new Operator("*", 2));

        Queue<MathSymbol> result = _symbolSorter.Sort(symbols);
        
        Assert.That(result.IsDeepEqual(expected));
    }

    [Test]
    public void SortTokens_InvalidLeftBracket_ShouldThrowException()
    {
        List<MathSymbol> symbols =
        [
            new Bracket("("),
            new Operand("a", 5),
            new Operator("+", 1),
            new Operand("b", 10),
            new Bracket("("),
            new Operator("*", 2),
            new Operand("c", 20)
        ];
        
        Assert.That(() => _symbolSorter.Sort(symbols), Throws.TypeOf<SymbolSorterException>()
            .With.Message.EqualTo("Mismatched left bracket found"));
    }
    
    [Test]
    public void SortTokens_InvalidRightBracket_ShouldThrowException()
    {
        List<MathSymbol> symbols =
        [
            new Bracket("("),
            new Operand("a", 5),
            new Operator("+", 1),
            new Operand("b", 10),
            new Bracket(")"),
            new Operator("*", 2),
            new Operand("c", 20),
            new Bracket(")")
        ];
        
        Assert.That(() => _symbolSorter.Sort(symbols), Throws.TypeOf<SymbolSorterException>()
            .With.Message.EqualTo("Mismatched right bracket found"));
    }
}
using GTQPL7.Classes;
using GTQPL7.Exceptions;
using GTQPL7.Utils.ResultDisplayers;

using NSubstitute;

namespace GTQPL7_Tests.Utils.RpnEvaluator;

[TestFixture]
public class RpnEvaluatorTests
{
    private GTQPL7.Utils.RpnEvaluator.RpnEvaluator _rpnEvaluator;
    private IResultDisplayer _resultDisplayer;

    [OneTimeSetUp]
    public void Setup()
    {
        _resultDisplayer = Substitute.For<IResultDisplayer>();
        _rpnEvaluator = new GTQPL7.Utils.RpnEvaluator.RpnEvaluator(_resultDisplayer);
    }

    [Test]
    public void Evaluate_SimpleExpression_ShouldReturnCorrectResult()
    {
        // det(a * A + B)
        Queue<MathSymbol> rpn = new Queue<MathSymbol>();
        rpn.Enqueue(new Operand("a", 2));
        rpn.Enqueue(new MatrixOperand("A", new Matrix(2, 2, 
            [1, 2, 3, 4])));
        rpn.Enqueue(new Operator("*", 2));
        rpn.Enqueue(new MatrixOperand("B", new Matrix(2, 2, 
            [-1, 2, -5, 7])));
        rpn.Enqueue(new Operator("+", 1));
        rpn.Enqueue(new Operator("det", 3));
        
        _rpnEvaluator.Evaluate(rpn);
        
        _resultDisplayer.Received().DisplayResult("Result is:" + System.Environment.NewLine + 9);
    }

    [Test]
    public void Evaluate_ExpressionWithMatMulAndTranspose_ShouldReturnCorrectResult()
    {
        // A * trans(B) * trans(C)
        Queue<MathSymbol> rpn = new Queue<MathSymbol>();
        rpn.Enqueue(new MatrixOperand("A", new Matrix(4, 3, 
            [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12])));
        rpn.Enqueue(new MatrixOperand("B", new Matrix(2, 3, 
            [-1, 2, -5, 7, -2, -2])));
        rpn.Enqueue(new Operator("trans", 3));
        rpn.Enqueue(new Operator("*", 2));
        rpn.Enqueue(new MatrixOperand("C", new Matrix(1, 2, 
            [-1, -2])));
        rpn.Enqueue(new Operator("trans", 3));
        rpn.Enqueue(new Operator("*", 2));
        const string result = "18 \n12 \n6 \n0 \n";
        
        _rpnEvaluator.Evaluate(rpn);
        
        _resultDisplayer.Received().DisplayResult("Result is:" + System.Environment.NewLine + result);
    }

    [Test]
    public void Evaluate_InvertIdentityMatrix_ShouldReturnCorrectResult()
    {
        // inv(A)
        Queue<MathSymbol> rpn = new Queue<MathSymbol>();
        rpn.Enqueue(new MatrixOperand("A", new Matrix(2, 2, 
            [1, 0, 0, 1])));
        rpn.Enqueue(new Operator("inv", 3));
        const string result = "1 0 \n0 1 \n";
        
        _rpnEvaluator.Evaluate(rpn);
        
        _resultDisplayer.Received().DisplayResult("Result is:" + System.Environment.NewLine + result);
    }
    
    [Test]
    public void Evaluate_InvertArbitraryMatrix_ShouldReturnCorrectResult()
    {
        // inv(A)
        Queue<MathSymbol> rpn = new Queue<MathSymbol>();
        rpn.Enqueue(new MatrixOperand("A", new Matrix(3, 3, 
            [-1, 2, 0, -2, 5, 1, 3, -6, -1])));
        rpn.Enqueue(new Operator("inv", 3));
        const string result = "1 2 2 \n1 1 1 \n-3 0 -1 \n";
        
        _rpnEvaluator.Evaluate(rpn);
        
        _resultDisplayer.Received().DisplayResult("Result is:" + System.Environment.NewLine + result);
    }

    [Test]
    public void Evaluate_InvalidOperandsForAddition_ShouldThrowException()
    {
        // a + A
        Queue<MathSymbol> rpn = new Queue<MathSymbol>();
        rpn.Enqueue(new Operand("a", 2));
        rpn.Enqueue(new MatrixOperand("A"));
        rpn.Enqueue(new Operator("+", 1));
        const string message = "Could not add GTQPL7.Classes.Operand to GTQPL7.Classes.MatrixOperand";

        Assert.That(() => _rpnEvaluator.Evaluate(rpn),
            Throws.TypeOf<RpnEvaluatorException>().With.Message.EqualTo(message));
    }
    
    [Test]
    public void Evaluate_InvalidOperandsForMultiplication_ShouldThrowException()
    {
        // A * a
        Queue<MathSymbol> rpn = new Queue<MathSymbol>();
        rpn.Enqueue(new MatrixOperand("A"));
        rpn.Enqueue(new Operand("a"));
        rpn.Enqueue(new Operator("*", 2));
        const string message = "Could not multiply GTQPL7.Classes.Operand by GTQPL7.Classes.MatrixOperand";

        Assert.That(() => _rpnEvaluator.Evaluate(rpn),
            Throws.TypeOf<RpnEvaluatorException>().With.Message.EqualTo(message));
    }
    
    [Test]
    public void Evaluate_TooFewOperandForOperator_ShouldThrowException()
    {
        // a +
        Queue<MathSymbol> rpn = new Queue<MathSymbol>();
        rpn.Enqueue(new Operand("a"));
        rpn.Enqueue(new Operator("+", 1));
        const string message = "Too few operands for addition";

        Assert.That(() => _rpnEvaluator.Evaluate(rpn),
            Throws.TypeOf<RpnEvaluatorException>().With.Message.EqualTo(message));
    }

    [Test]
    public void Evaluate_InvalidExpression_ShouldThrowException()
    {
        // a + b c d
        Queue<MathSymbol> rpn = new Queue<MathSymbol>();
        rpn.Enqueue(new Operand("a"));
        rpn.Enqueue(new Operand("b"));
        rpn.Enqueue(new Operand("c"));
        rpn.Enqueue(new Operand("d"));
        rpn.Enqueue(new Operator("+", 1));
        const string message = "The given expression could not be evaluated";
        
        Assert.That(() => _rpnEvaluator.Evaluate(rpn), Throws.TypeOf<RpnEvaluatorException>()
            .With.Message.EqualTo(message));
    }
}
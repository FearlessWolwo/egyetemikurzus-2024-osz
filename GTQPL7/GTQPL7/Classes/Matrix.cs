using System.Numerics;

namespace GTQPL7.Classes;

public class Matrix
{
    public Matrix(int rows, int columns, double[] values)
    {
        if (values.Length != rows * columns)
        {
            throw new ArgumentException("Matrix size must be the same size");
        }
        RowCount = rows;
        ColumnCount = columns;
        Values = new double[rows][];
        for (int i = 0; i < Values.Length; i++)
        {
            Values[i] = new double[columns];
            for (int j = 0; j < Values[i].Length; j++)
            {
                Values[i][j] = values[i * columns + j];
            }
        }
    }
    
    public int RowCount { get; }
    public int ColumnCount { get; }
    public double[][] Values { get; }
}
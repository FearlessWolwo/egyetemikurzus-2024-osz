using System.Numerics;

namespace GTQPL7.Classes;

public class Matrix<T> where T : ISignedNumber<T>
{
    public Matrix(int rows, int columns, T[] values)
    {
        if (values.Length != rows * columns)
        {
            throw new ArgumentException("Matrix size must be the same size");
        }
        RowCount = rows;
        ColumnCount = columns;
        Values = new T[rows][];
        for (int i = 0; i < Values.Length; i++)
        {
            Values[i] = new T[columns];
            for (int j = 0; j < Values[i].Length; j++)
            {
                Values[i][j] = values[i * columns + j];
            }
        }
    }
    
    public int RowCount { get; }
    public int ColumnCount { get; }
    public T[][] Values { get; }
}
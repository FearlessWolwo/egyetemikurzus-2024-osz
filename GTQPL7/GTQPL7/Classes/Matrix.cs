using System.Text;

namespace GTQPL7.Classes;

public record Matrix
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
        for (int row = 0; row < Values.Length; row++)
        {
            Values[row] = new double[columns];
            for (int col = 0; col < Values[row].Length; col++)
            {
                Values[row][col] = values[row * columns + col];
            }
        }
    }

    public Matrix(int rows, int columns, double[][] values)
    {
        if (values.Length != rows || values[0].Length != columns)
        {
            throw new ArgumentException("Matrix size must be the same size");
        }
        RowCount = rows;
        ColumnCount = columns;
        Values = values;
    }
    
    public int RowCount { get; }
    public int ColumnCount { get; }
    public double[][] Values { get; }

    [System.Runtime.CompilerServices.IndexerName("ValueIndexer")]
    public double this[int row, int column]
    {
        get => Values[row][column];
    }

    public Matrix MultiplyByScalar(double scalar)
    {
        int rowCount = RowCount;
        int columnCount = ColumnCount;
        double[] values = new double[rowCount * columnCount];
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                values[row * columnCount + col] = scalar * this[row, col];
            }
        }
        return new Matrix(rowCount, columnCount, values);
    }

    public static Matrix operator +(Matrix lhs, Matrix rhs)
    {
        if (lhs.RowCount != rhs.RowCount || lhs.ColumnCount != rhs.ColumnCount)
        {
            throw new ArgumentException("Matrix size mismatch");
        }
        
        int rowCount = lhs.RowCount;
        int columnCount = lhs.ColumnCount;
        double[] values = new double[rowCount * columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                values[row * columnCount + col] = lhs[row, col] + rhs[row, col];
            }
        }
        
        return new Matrix(rowCount, columnCount, values);
    }

    public static Matrix operator -(Matrix lhs, Matrix rhs)
    {
        return lhs + -rhs;
    }

    public static Matrix operator -(Matrix matrix)
    {
        int rowCount = matrix.RowCount;
        int columnCount = matrix.ColumnCount;
        double[] values = new double[rowCount * columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                values[row * columnCount + col] = -1 * matrix[row, col];
            }
        }
        
        return new Matrix(rowCount, columnCount, values);
    }

    public static Matrix operator *(Matrix lhs, Matrix rhs)
    {
        
        if (lhs.ColumnCount != rhs.RowCount)
        {
            throw new ArgumentException("Matrix size mismatch");
        }
        
        int rowCount = lhs.RowCount;
        int columnCount = rhs.ColumnCount;
        double[] values = new double[rowCount * columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                for (int k = 0; k < lhs.ColumnCount; k++)
                {
                    values[row * columnCount + col] += lhs[row, k] * rhs[k, col];
                }
            }
        }
        
        return new Matrix(rowCount, columnCount, values);
    }
    
    public Matrix Transpose()
    {
        double[] values = new double[ColumnCount * RowCount];

        for (int row = 0; row < ColumnCount; row++)
        {
            for (int col = 0; col < RowCount; col++)
            {
                values[row * RowCount + col] = this[col, row];
            }
        }
        
        return new Matrix(ColumnCount, RowCount, values);
    }
    
    public double Determinant()
    {
        return GaussElimination().Determinant;
    }

    public Matrix Invert()
    {
        return GaussElimination().Inverse ?? throw new ArgumentException("Matrix is singular");
    }

    private GaussEliminationResult GaussElimination()
    {
        if (RowCount != ColumnCount)
        {
            throw new ArgumentException("Matrix is not square");
        }
        double[][] copied = CopyValues();
        double[][] inverse = new double[RowCount][];
        for (int row = 0; row < RowCount; row++)
        {
            inverse[row] = new double[ColumnCount];
            inverse[row][row] = 1;
        }
        double determinant = 1;
        for (int row = 0; row < RowCount; row++)
        {
            // find non-zero element in rowth column
            int selectedRow = row;
            for (int remainingRow = 0; remainingRow < RowCount; remainingRow++)
            {
                if (Math.Abs(copied[remainingRow][row]) > Math.Abs(copied[selectedRow][row]))
                {
                    selectedRow = remainingRow;
                }
            }

            // if there are no non-zero elements return "0"
            if (copied[selectedRow][row].Equals(0))
            {
                return new GaussEliminationResult(null, 0);
            }

            // swap selected row with rowth row
            for (int col = 0; col < ColumnCount; col++)
            {
                (copied[selectedRow][col], copied[row][col]) = (copied[row][col], copied[selectedRow][col]);
                (inverse[col][selectedRow], inverse[col][row]) = (inverse[col][row], inverse[col][selectedRow]);
            }
            
            // if rows were different negate determinant
            if (selectedRow != row)
            {
                determinant *= -1;
            }

            determinant *= copied[row][row];

            for (int col = row + 1; col < ColumnCount; col++)
            {
                copied[row][col] /= copied[row][row];
                inverse[row][col] /= inverse[row][row];
            }

            for (int otherRow = 0; otherRow < RowCount; otherRow++)
            {
                if (otherRow == row)
                {
                    continue;
                }
                for (int col = row + 1; col < RowCount; col++)
                {
                    copied[otherRow][col] -= copied[row][col] * copied[otherRow][row];
                }
                for (int col = row + 1; col < RowCount; col++)
                {
                    inverse[otherRow][col] -= inverse[row][col] * inverse[otherRow][row];
                }
            }
        }
        return new GaussEliminationResult(new Matrix(ColumnCount, RowCount, inverse), determinant);
    }
    private double[][] CopyValues()
    {
        double[][] copied = new double[RowCount][];
        
        for (int row = 0; row < RowCount; row++)
        {
            copied[row] = new double[ColumnCount];
            for (int col = 0; col < ColumnCount; col++)
            {
                copied[row][col] = this[row, col];
            }
        }

        return copied;
    }

    private record GaussEliminationResult
    {
        public Matrix? Inverse { get; }
        public double Determinant { get; }

        public GaussEliminationResult(Matrix? inverse, double determinant)
        {
            this.Inverse = inverse;
            this.Determinant = determinant;
        }
    }

    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                result.Append(this[i, j]).Append(' ');
            }
            result.AppendLine();
        }
        return result.ToString();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace TIPE3
{
    public class MultivariatePolynomialMatrix
    {
        MultivariatePolynomial[,] matrix;
        public int Columns
        {
            get
            {
                return matrix.GetLength(0);
            }
        }
        public int Rows
        {
            get {
                return matrix.GetLength(1);
            }
        }
        public MultivariatePolynomialMatrix(MultivariatePolynomial[,] m)
        {
            matrix = m;
        }
        public MultivariatePolynomial this[int j,int i]
        {
            get
            {
                return matrix[j, i];
            }
        }
        public static MultivariatePolynomialMatrix operator +(MultivariatePolynomialMatrix m1, MultivariatePolynomialMatrix m2) => Add(m1, m2);
        public static MultivariatePolynomialMatrix operator *(MultivariatePolynomialMatrix m1, MultivariatePolynomialMatrix m2) => Multiply(m1, m2);
        public static MultivariatePolynomialMatrix operator -(MultivariatePolynomialMatrix m) => Multiply(-1+0*Complex.j, m);
        public static MultivariatePolynomialMatrix operator -(MultivariatePolynomialMatrix m1, MultivariatePolynomialMatrix m2) => m1 + (-m2);
        public static MultivariatePolynomialMatrix Add(MultivariatePolynomialMatrix m1, MultivariatePolynomialMatrix m2)
        {
            Debug.Assert(m1.Columns == m2.Columns && m1.Rows == m2.Rows);
            MultivariatePolynomial[,] sumMatrix = new MultivariatePolynomial[m1.Columns, m1.Rows];
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    sumMatrix[j,i] = m1[j,i] + m2[j,i];
                }
            }
            return new MultivariatePolynomialMatrix(sumMatrix);

        }
        public static MultivariatePolynomialMatrix Multiply(MultivariatePolynomialMatrix m1, MultivariatePolynomialMatrix m2)
        {
            Debug.Assert(m1.Columns == m2.Rows);
            MultivariatePolynomial[,] productMatrix = new MultivariatePolynomial[m1.Rows, m2.Columns];
            for (int j = 0; j < m2.Columns; j++)
            {
                for (int i = 0; i < m1.Rows; i++)
                {
                    MultivariatePolynomial c = new MultivariatePolynomial(new Dictionary<int[], Complex>());
                    for (int k = 0; k < m1.Rows; k++)
                    {
                        c = c + m1[k, i] + m2[j, k];
                    }
                    productMatrix[j, i] = c;
                }
            }
            return new MultivariatePolynomialMatrix(productMatrix);
        }
        public static MultivariatePolynomialMatrix Multiply(Complex c, MultivariatePolynomialMatrix m)
        {
           
            MultivariatePolynomial[,] productMatrix = new MultivariatePolynomial[m.Rows, m.Columns];
            for (int j = 0; j < m.Columns; j++)
            {
                for (int i = 0; i < m.Rows; i++)
                {
                    productMatrix[j, i] = c * m[j, i];
                }
            }
            return new MultivariatePolynomialMatrix(productMatrix);
        }

    }
}

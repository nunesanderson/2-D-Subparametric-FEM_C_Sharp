using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WindowsFormsApplication1
{
    public class MatrixOper
    {

        /// <summary>
        /// Retorna a soma de duas matrizes
        /// </summary>
        /// <param name="Mat1">Primeira matriz</param>
        /// <param name="Mat2">Segunda matriz</param>
        /// <returns></returns>
        public double[,] Sum(double[,] Mat1, double[,] Mat2)
        {
            int Mat1L = Mat1.GetLength(0);
            int Mat1C = Mat1.GetLength(1);
            int Mat2L = Mat2.GetLength(0);
            int Mat2C = Mat2.GetLength(1);

            int minL;
            int minC;

            if (Mat1L <= Mat2L)
            {
                minL = Mat1L;
            }
            else
            {
                minL = Mat2L;
            }

            if (Mat1C <= Mat2C)
            {
                minC = Mat1C;
            }
            else
            {
                minC = Mat2C;
            }

            double[,] matrix = new double[minL, minC];
            //Soma dos vetores
            for (int i = 0; i < minL; i++)
            {
                for (int j = 0; j < minC; j++)
                {
                    matrix[i, j] = Mat1[i, j] + Mat2[i, j];
                }
            }
            return matrix;
        }

        /// <summary>
        /// Converte uma matriz CSML para um array double
        /// </summary>
        /// <param name="inputMatrix"></param>
        /// <returns></returns>
        public double[,] Conv_CSMLtoDouble(Matrix inputMatrix)
        {

            int row = inputMatrix.RowCount;
            int col = inputMatrix.ColumnCount;
            double[,] outputMatrix = new double[row, col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Complex num = inputMatrix[i + 1, j + 1];
                    if (Complex.Abs(num) == 0)
                    {
                        outputMatrix[i, j] = 0;
                    }
                    else
                    {
                        outputMatrix[i, j] = Complex.Abs(num) * Math.Cos(Complex.Arg(num));
                    }
                }
            }
            return outputMatrix;
        }

        /// <summary>
        /// Converte uma array double em uma matriz CSML
        /// </summary>
        /// <param name="inputMatrix"></param>
        /// <returns></returns>
        public Matrix Conv_DoubleToCSML(double[,] inputMatrix)
        {
            int row = inputMatrix.GetLength(0);
            int col = inputMatrix.GetLength(1);

            Matrix outputMatrix = new Matrix(row, col);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Complex number = new Complex(inputMatrix[i, j], 0);

                    outputMatrix[i + 1, j + 1] = number;
                }
            }
            return outputMatrix;
        }

        /// <summary>
        ///Transpõe uma matriz
        /// </summary>
        /// <param name="inputMatrix">Matriz a ser transposta</param>
        /// <returns></returns>

        public double[,] Transpose(double[,] inputMatrix)
        {
            int row = inputMatrix.GetLength(0);
            int col = inputMatrix.GetLength(1);
            double[,] outputMatrix = new double[col, row];
            int j_in;
            int i_in;
            for (int j = 0; j < row; j++)
            {
                for (int i = 0; i < col; i++)
                {
                    j_in = i;
                    i_in = j;
                    outputMatrix[i, j] = inputMatrix[i_in, j_in];
                }
            }

            return outputMatrix;
        }
        /// <summary>
        /// Multiplica duas matrizes: Mat1[lin1,col1] x Mat2[lin2,col2]
        /// </summary>
        /// <param name="Mat1">Primeira matriz</param>
        /// <param name="Mat2">Segunda matriz</param>
        /// <returns> M x M [lin1,col2]</returns>

        public double[,] Multiply_MM(double[,] Mat1, double[,] Mat2)
        {
            int row1 = Mat1.GetLength(0);
            int col1 = Mat1.GetLength(1);
            int row2 = Mat2.GetLength(0);
            int col2 = Mat2.GetLength(1);
            int rowOut = row1;
            int colOut = col2;
            double[,] outputMatrix = new double[rowOut, colOut];

            for (int i = 0; i < rowOut; i++)
            {
                for (int j = 0; j < colOut; j++)
                {

                    for (int k = 0; k < col1; k++)
                    {
                        outputMatrix[i, j] = Mat1[i, k] * Mat2[k, j] + outputMatrix[i, j];
                    }
                }
            }
            return outputMatrix;
        }

        /// <summary>
        /// Multiplica um número real A pela matriz M
        /// </summary>
        /// <param name="A">Número real</param>
        /// <param name="Mat">Matriz</param>
        /// <returns>A x B</returns>
        public double[,] Multiply_AM(double A, double[,] Mat)
        {
            int row = Mat.GetLength(0);
            int col = Mat.GetLength(1);
            double[,] outputMatrix = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    outputMatrix[i, j] = Mat[i, j] * A;
                }
            }
            return outputMatrix;
        }

        /// <summary>
        /// Aplica a eliminação de Gauss-Jordan na resolução do sistema matricial
        /// </summary>
        /// <param name="Mat1">Matriz 1 [n,n]</param>
        /// <param name="Mat2"> Matriz 2 [n,1]</param>
        /// <returns></returns>

        public double[,] Gauss_JordanElm(double[,] Mat1, double[,] Mat2)
        {
            int row1 = Mat1.GetLength(0);
            int col1 = Mat1.GetLength(1);
            int row2 = Mat2.GetLength(0);
            int col2 = Mat2.GetLength(1);
            double[,] outputMatrix = new double[row1, 1];
            double[,] matWork = new double[row1, col1 + 1];

            //Aplicação da Eliminação de Gauss-Jordan
            int colCount = 0;
            int lineCount = 0;
            int lineCount2 = 0;
            for (int jc = 0; jc < col1 - 1; jc++)
            {
                for (int i = 1 + lineCount2; i < row1; i++)
                {
                    double div = Mat1[i, colCount] / Mat1[lineCount, colCount];
                    Mat2[i, 0] = Mat2[i, 0] + Mat2[lineCount, 0] * -div;

                    for (int j = 0; j < col1; j++)
                    {
                        Mat1[i, j] = Mat1[i, j] + Mat1[lineCount, j] * -div;
                    }
                }
                lineCount++;
                lineCount2++;
                colCount++;
            }

            double div2 = 0;
            for (int i = 0; i < row1; i++)
            {
                for (int j = 0; j < col1; j++)
                {
                    if (i == j)
                    {
                        div2 = Mat1[i, j];
                    }
                }
                Mat2[i, 0] = Mat2[i, 0] / div2;
                for (int ja = 0; ja < col1; ja++)
                {
                    Mat1[i, ja] = Mat1[i, ja] / div2;
                }
            }

            return Mat2;
        }


        /// <summary>

        /// GaussianElimination() 

        /// Gaussian elimination is a method for solving matrix equations

        /// By Harvey Triana - http://vexpert.mvps.org/articles/GJE.htm

        /// </summary>

        /// <param name="a"> The matrix</param>

        /// <param name="r"> The solution array</param>

        /// <returns>Success function</returns>


        public bool Gauss_JordanElm2(double[,] a, double[,] r)
        {

           
            double t, s;

            int i, l, j, k, m, n;

            try
            {

                n = r.GetLength(0)-1;

                m = n + 1;

                for (l = 0; l <= n - 1; l++)
                {

                    j = l;

                    for (k = l + 1; k <= n; k++)
                    {

                        if (!(Math.Abs(a[j, l]) >= Math.Abs(a[k, l]))) j = k;

                    }

                    if (!(j == l))
                    {

                        for (i = 0; i <= m; i++)
                        {

                            t = a[l, i];

                            a[l, i] = a[j, i];

                            a[j, i] = t;

                        }

                    }

                    for (j = l + 1; j <= n; j++)
                    {

                        t = (a[j, l] / a[l, l]);

                        for (i = 0; i <= m; i++) a[j, i] -= t * a[l, i];

                    }

                }

                r[n,0] = a[n, m] / a[n, n];

                for (i = 0; i <= n - 1; i++)
                {

                    j = n - i - 1;

                    s = 0;

                    for (l = 0; l <= i; l++)
                    {

                        k = j + l + 1;

                        s += a[j, k] * r[k,0];

                    }

                    r[j,0] = ((a[j, m] - s) / a[j, j]);

                }

               
                return true;

            }

            catch
            {


                return false;

            }

        }


        /// <summary>
        /// Escreve um dada matriz em determinado arquivo
        /// </summary>
        /// <param name="Mat">Matriz</param>
        /// <param name="filename">Nome do arquivo</param>


        public void WriteToFile(double[,] Mat, string filename)
        {
            int row = Mat.GetLength(0);
            int col = Mat.GetLength(1);

            using (StreamWriter Write = new StreamWriter(filename + ".txt", false))
            {
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        Write.Write(Mat[i, j].ToString());
                        Write.Write('\t');

                    }
                    Write.WriteLine();

                }


            }
        }

        /// <summary>
        /// Retorna a coordenada x do baricentro de um triângulo
        /// </summary>
        /// <param name="Mat">Matriz contendo as coordenadas dos pontos do triangulo</param>
        /// <returns></returns>
        public double Triangle_Centroid(double[,] Mat)
        {
            
            double x_coord=0 ;
            double y_coord=0 ;

            for (int i = 0; i < 3; i++)
            {
                x_coord = Mat[i, 0] + x_coord;
                y_coord = Mat[i, 1] + y_coord;
            }

            x_coord = x_coord / 3;
            y_coord = y_coord / 3;

            return x_coord;
        }

    }
}

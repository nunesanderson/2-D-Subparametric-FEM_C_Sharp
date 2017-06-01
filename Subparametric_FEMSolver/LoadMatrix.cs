using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class LoadMatrix
    {
        string delim = ";";

        /// <summary>
        /// Matriz com as coordenadas dos nós
        /// </summary>
        public void LoadCoordenates(out int ColCoordenates, out int RowCoordenates, out double[,] coordenates)
        {

            LerMatriz ler = new LerMatriz();
            int maxcolunas;
            int maxlinhas;
            double[,] matriz = new double[1, 1];

            ler.LerMatrizArquivo("coordenates.csv", delim, out maxcolunas, out  maxlinhas, out matriz);

            ColCoordenates = maxcolunas;
            RowCoordenates = maxlinhas;
            coordenates = matriz;
        }

        /// <summary>
        /// Matriz com os elementos de cada triângulo
        /// </summary>
        public void LoadVertexes(out int ColVertexes, out int RowVertexs, out double[,] vertexes)
        {

            LerMatriz ler = new LerMatriz();
            int maxcolunas;
            int maxlinhas;
            double[,] matriz = new double[1, 1];

            ler.LerMatrizArquivo("vertexes.csv", delim, out maxcolunas, out  maxlinhas, out matriz);

            ColVertexes = maxcolunas;
            RowVertexs = maxlinhas;
            vertexes = matriz;
        }

        /// <summary>
        /// Matriz com índice do material e fonte de cada elemento
        /// </summary>
        public void LoadProperties(out int ColProperties, out int RowProperties, out double[,] properties)
        {

            LerMatriz ler = new LerMatriz();
            int maxcolunas;
            int maxlinhas;
            double[,] matriz = new double[1, 1];

            ler.LerMatrizArquivo("properties.csv", delim, out maxcolunas, out  maxlinhas, out matriz);

            ColProperties = maxcolunas;
            RowProperties = maxlinhas;
            properties = matriz;
        }
        /// <summary>
        /// Matriz com as condições de contorno
        /// </summary>
        public void LoadBoundCond(out int ColBoundCond, out int RowBoundCond, out double[,] boundcond)
        {

            LerMatriz ler = new LerMatriz();
            int maxcolunas;
            int maxlinhas;
            double[,] matriz = new double[1, 1];

            ler.LerMatrizArquivo("boundcond.csv", delim, out maxcolunas, out  maxlinhas, out matriz);

            ColBoundCond = maxcolunas;
            RowBoundCond = maxlinhas;
            boundcond = matriz;
        }

        /// <summary>
        /// Matriz de materiais
        /// </summary>
        public void LoadMat(out int ColMat, out int RowMat, out double[,] mat)
        {

            LerMatriz ler = new LerMatriz();
            int maxcolunas;
            int maxlinhas;
            double[,] matriz = new double[1, 1];

            ler.LerMatrizArquivo("material.csv", delim, out maxcolunas, out  maxlinhas, out matriz);

            ColMat = maxcolunas;
            RowMat = maxlinhas;
            mat = matriz;
        }




    }
}

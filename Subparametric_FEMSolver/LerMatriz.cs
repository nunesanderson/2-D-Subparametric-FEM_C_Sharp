using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class LerMatriz
    {
        public void LerMatrizArquivo(string filename, string delim, out int maxcolunas, out int maxlinhas, out double[,] matriz)
        {


            StreamReader sr = new StreamReader(filename);//le o arquivo como um bloco de strings

            string line;
            int ContadorLinhas = 0;
            int ContadorColunas = 0;
            maxcolunas = 0;
            List<string> lista = new List<string>();
            maxlinhas = 0;

            while ((line = sr.ReadLine()) != null)//read the file until there are lines to read
            {
                if (line[0] != 'c')
                {
                    foreach (string lineeach in line.Split(delim.ToCharArray()))
                    {
                        if (!string.IsNullOrEmpty(lineeach))
                        {
                        }
                    }

                    ContadorColunas = 0;
                    while (ContadorColunas < line.Split(delim.ToCharArray()[0]).Length)
                    {

                        ContadorColunas++;
                    }

                    if (maxcolunas < ContadorColunas)
                    {
                        maxcolunas = ContadorColunas;

                    }


                    lista.Add(line);


                    ContadorColunas = 0;
                    ContadorLinhas++;
                    maxlinhas = ContadorLinhas;
                }

            }
            // write on console the matrix data, for each one


            ////////////////////////////////////////////////////////

            matriz = new double[maxlinhas, maxcolunas];
            ContadorColunas = 0;
            ContadorLinhas = 0;

            for (ContadorLinhas = 0; ContadorLinhas < maxlinhas; ContadorLinhas++)
            {

                int teste = lista[ContadorColunas].Split(delim.ToCharArray()[0]).Length;
                for (ContadorColunas = 0; ContadorColunas < teste; ContadorColunas++)
                {

                    string teste3 = lista[ContadorLinhas].Split(delim.ToCharArray()[0])[ContadorColunas];

                    matriz[ContadorLinhas, ContadorColunas] = double.Parse(teste3);

                }
                ContadorColunas = 0;
            }

        }
    }
}

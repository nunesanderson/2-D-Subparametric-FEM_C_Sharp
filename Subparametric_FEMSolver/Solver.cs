using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class Solver
    {
        /// <summary>
        /// Inicializa a variável load
        /// </summary>
        LoadMatrix load = new LoadMatrix();

        /////////////////////////////////////////////////////////////////
        // Variáveis retornadas por LoadMatriz
        int _colCoordenates;
        int _rowCoordenates;
        double[,] coordenates;
        int _colVertexes;
        int _rowVertexes;
        int _colProperties;
        int _rowProperties;
        double[,] properties;
        int _colBoundCond;
        int _rowBoundCond;
        double[,] boundcond;
        int _colMat;
        int _rowMat;
        double[,] mat;


        /// <summary>
        /// Iniacializa a variável oper como MatrixOper
        /// </summary>
        MatrixOper oper = new MatrixOper();

        /// <summary>
        ///Método que resolve o problema
        /// </summary>
        /// 
        public double[,] Run_solver()
        {

            //Carrega as matrizes dos arquivos .csv
            load.LoadCoordenates(out _colCoordenates, out _rowCoordenates, out coordenates);
            load.LoadProperties(out _colProperties, out _rowProperties, out properties);
            load.LoadVertexes(out _colVertexes, out _rowVertexes, out vertexes);
            load.LoadBoundCond(out _colBoundCond, out _rowBoundCond, out boundcond);
            load.LoadMat(out _colMat, out _rowMat, out mat);

            /////////////////////////////////////////////////////////////////
            //Seta o tipo de problema a ser resolvido 
            //Eletrostático=0
            //Magnetostático = 2
            int problemType = 2;
            //Simetria Axi-simétrico?
            bool axis_sym = false;

            /////////////////////////////////////////////////////////////////
            //Seta a quantidade de nós e de elementos
            int nodes = _rowCoordenates;
            int elements = _rowVertexes;

            /////////////////////////////////////////////////////////////////
            //Inicializa as matrizes globais
            double[,] globalMatrix = new double[nodes, nodes];
            double[,] globalMatrix2 = new double[nodes, 1];

            /////////////////////////////////////////////////////////////////
            //Carrega os valores de x e y em duas listas
            List<double> x = new List<double>();
            List<double> y = new List<double>();

            x = new List<double>();
            y = new List<double>();

            for (int i = 0; i < nodes; i++)
            {
                x.Add(coordenates[i, 0]);
                y.Add(coordenates[i, 1]);
            }

            /////////////////////////////////////////////////////////////////
            //Definição dos pontos de integração
            //Dados para os 7 pontos de integração
            int r = 7;
            double a = 0.470142064;
            double b = 0.101286507;
            double w1 = 0.066197076;
            double w2 = 0.062969590;

            List<double> u_int = new List<double>();
            List<double> v_int = new List<double>();
            List<double> w_int = new List<double>();
            u_int.Add(1D / 3);
            u_int.Add(a);
            u_int.Add(1 - 2 * a);
            u_int.Add(a);
            u_int.Add(b);
            u_int.Add(1 - 2 * b);
            u_int.Add(b);

            v_int.Add(1D / 3);
            v_int.Add(a);
            v_int.Add(a);
            v_int.Add(1 - 2 * a);
            v_int.Add(b);
            v_int.Add(b);
            v_int.Add(1 - 2 * b);

            w_int.Add(9D / 80);
            w_int.Add(w1);
            w_int.Add(w1);
            w_int.Add(w1);
            w_int.Add(w2);
            w_int.Add(w2);
            w_int.Add(w2);

            #region loop principal



            for (int i = 0; i < elements; i++)
            {
                ///////////////////////////////////////////////////////////////////////
                //Contém os nós globais de cada elemento i
                List<int> vert = new List<int>();

                for (int k = 0; k < 6; k++)
                {
                    vert.Add(Convert.ToInt32(vertexes[i, k]) - 1);
                }

                ///////////////////////////////////////////////////////////////////////
                //Definição, inversão e determinante da matriz Jacobiana
                double[,] j = new double[2, 2];
                double[,] factj = new double[2, 3] {
                { -1, 1, 0 }, { -1, 0, 1 } 
                };

                double[,] coordj = new double[3, 2] { 
                { x[vert[0]], y[vert[0]] },
                { x[vert[2]], y[vert[2]] },
                {x[vert[4]], y[vert[4]] }
                };
                j = oper.Multiply_MM(factj, coordj);

                double detJ = j[0, 0] * j[1, 1] - j[1, 0] * j[0, 1];

                Matrix j_cs = oper.Conv_DoubleToCSML(j);
                Matrix inv_J_cs = j_cs.Inverse();
                double[,] invJ = oper.Conv_CSMLtoDouble(inv_J_cs);

                ///////////////////////////////////////////////////////////////////////
                //Calcula o baricentro 

                double r0 = 1;
                if (axis_sym)
                {
                    r0 = oper.Triangle_Centroid(coordj);

                }

                ///////////////////////////////////////////////////////////////////////
                //Seta a característica do material de cada elemento
                double v0 = 0;
                double mat_prop = 0;

                if (problemType == 0)
                {
                    v0 = 8.85 * Math.Pow(10, -12);
                    mat_prop = mat[Convert.ToInt32(properties[i, 0]), problemType] * v0 / r0;
                }
                else
                {
                    if (problemType == 2)
                    {
                        // v0 = 1 / (4 * Math.PI * Math.Pow(10, -7));
                        v0 = 795800;
                        mat_prop = r0 * (1 / mat[Convert.ToInt32(properties[i, 0]), problemType]) * v0 / r0;
                    }
                }


                ///////////////////////////////////////////////////////////////////////
                //Integração no lado esquerdo
                //Integral Trans(InvJ*GradN)*InvJ*GradN*det(J)*dudv
                double[,] npart = new double[1, 6];
                double[,] gradNpart = new double[2, 6];
                double[,] integ_part = new double[6, 6];
                double[,] integ = new double[6, 6];
                double[,] n = new double[6, 1];
                double t;
                double dudv = 0.5;

                //Loop para integrãção numérica do lado direito
                for (int pint = 0; pint < r; pint++)
                {
                    double u = u_int[pint];
                    double v = v_int[pint];
                    double w = w_int[pint];
                    t = 1 - u - v;

                    gradNpart[0, 0] = 1 - 4 * t;
                    gradNpart[0, 1] = 4 * (t - u);
                    gradNpart[0, 2] = -1 + (4 * u);
                    gradNpart[0, 3] = 4 * v;
                    gradNpart[0, 4] = 0;
                    gradNpart[0, 5] = -4 * v;
                    gradNpart[1, 0] = 1 - (4 * t);
                    gradNpart[1, 1] = -4 * u;
                    gradNpart[1, 2] = 0;
                    gradNpart[1, 3] = 4 * u;
                    gradNpart[1, 4] = -1 + (4 * v);
                    gradNpart[1, 5] = 4 * (t - v);

                    double[,] invJGradN = new double[2, 6];


                    invJGradN = oper.Multiply_MM(invJ, gradNpart);

                    // Integral do lado esquerdo
                    integ_part = oper.Multiply_AM(detJ * dudv * mat_prop * w, oper.Multiply_MM(oper.Transpose(invJGradN), invJGradN));
                    integ = oper.Sum(integ, integ_part);
                }

                ///////////////////////////////////////////////////////////////////////
                //Matriz global do lado esquerdo
                double[,] localMatrix = new double[nodes, nodes];
                for (int im = 0; im < 6; im++)
                {
                    for (int jm = 0; jm < 6; jm++)
                    {
                        localMatrix[vert[im], vert[jm]] = integ[im, jm];
                    }
                }
                globalMatrix = oper.Sum(globalMatrix, localMatrix);

                ///////////////////////////////////////////////////////////////////////
                //Integração no lado direito
                //Integral Trans[N]*[Js]*Det[J]*dudv

                double[,] integpart2 = new double[6, 1];
                double[,] integ2 = new double[6, 1];
                for (int pint = 0; pint < r; pint++)
                {
                    double u = u_int[pint];
                    double v = v_int[pint];
                    double w = w_int[pint];
                    t = 1 - u - v;

                    npart[0, 0] = -t * (1 - 2 * t);
                    npart[0, 1] = 4 * u * t;
                    npart[0, 2] = -u * (1 - 2 * u);
                    npart[0, 3] = 4 * u * v;
                    npart[0, 4] = -v * (1 - 2 * v);
                    npart[0, 5] = 4 * v * t;

                    integpart2 = oper.Multiply_AM(detJ * dudv * w * properties[i, 1], oper.Transpose(npart));
                    integ2 = oper.Sum(integ2, integpart2);
                }

                ///////////////////////////////////////////////////////////////////////
                //Matriz global do lado direito
                double[,] localMatrix2 = new double[nodes, 1];
                for (int im = 0; im < 6; im++)
                {

                    localMatrix2[vert[im], 0] = integ2[im, 0];
                }
                globalMatrix2 = oper.Sum(globalMatrix2, localMatrix2);
            }

            #endregion


            ///////////////////////////////////////////////////////////////////////
            //Verifica a simetria da matriz global do lado esquerdo
            double[,] test = new double[nodes, nodes];
            double[,] mat1 = new double[nodes, nodes];
            double[,] result = new double[nodes, nodes];
            result = oper.Sum(oper.Multiply_AM(-1, globalMatrix), globalMatrix);
            double teste = 0;

            for (int ii = 0; ii < nodes; ii++)
            {
                for (int jj = 0; jj < nodes; jj++)
                {
                    if (ii != jj)
                    {
                        teste = teste + result[ii, jj];
                    }
                }
            }

            ///////////////////////////////////////////////////////////////////////
            //Aplica as condições de contorno na matriz global do lado esquerdo
            for (int ibc = 0; ibc < nodes; ibc++)
            {
                for (int jbc = 0; jbc < nodes; jbc++)
                {
                    if (boundcond[ibc, 0] == 1)
                    {
                        ////Condição de contorno lado direito
                        globalMatrix2[ibc, 0] = boundcond[ibc, 1];

                        //Condição de contorno lado esquerdo
                        if (ibc == jbc)
                        {
                            globalMatrix[ibc, jbc] = 1;

                        }
                        else
                        {
                            globalMatrix[ibc, jbc] = 0;
                        }
                    }
                }
            }


            ///////////////////////////////////////////////////////////////////////
            //Resolução o sistema matricial

            double[,] matGJ = new double[nodes, nodes + 1];
            double[,] resultvector = new double[nodes, 1];

            // Cria a matriz de elementos nodes x nodes +1 para realizar a eliminação de Gauss Jordan
            for (int i = 0; i < nodes; i++)
            {
                for (int j = 0; j < nodes; j++)
                {
                    matGJ[i, j] = globalMatrix[i, j];
                }
            }

            for (int i_ = 0; i_ < nodes; i_++)
            {
                matGJ[i_, nodes] = globalMatrix2[i_, 0];

            }

            ////Escreve em arquivo as matrizes para solução do sistema linear
            //oper.WriteToFile(GlobalMatrix, "Integral_Lado_Esquerdo");
            //oper.WriteToFile(GlobalMatrix2, "Integral_Lado_Direito");
            //oper.WriteToFile(matGJ, "Matriz_para_Gauss_Jordan");


            //Resolução do sistema matricial por Eliminação de Gauss Jordan
            if (oper.Gauss_JordanElm2(matGJ, resultvector))
            {

                //AXI-SIMÉTRICO
                if (axis_sym)
                {
                    for (int i = 0; i < nodes; i++)
                    {
                        resultvector[i, 0] = resultvector[i, 0] * coordenates[i, 0];//multiplica o resultado por r do nó
                    }
                }

                //Exporta tabelas para desenho de malha e plotagem de campos
                oper.WriteToFile(coordenates, "out_coordenates");
                oper.WriteToFile(properties, "out_properties");
                oper.WriteToFile(vertexes, "out_vertexes");
                oper.WriteToFile(resultvector, "out_results");

                return resultvector;
            }
            else
            {
                return null;
            }
        }
    }
}

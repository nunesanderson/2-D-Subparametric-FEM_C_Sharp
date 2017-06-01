using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Solver solv = new Solver();
        MatrixOper oper = new MatrixOper();

        private void button1_Click(object sender, EventArgs e)
        {
            double[,] results;

            System.Diagnostics.Stopwatch cronometro = System.Diagnostics.Stopwatch.StartNew();
            results = solv.Run_solver();
            cronometro.Stop();

            int count = 0;
            int row = results.GetLength(0);

            richTextBox.Clear();

            richTextBox.AppendText("Cálculo resolvido em: " + cronometro.Elapsed.Milliseconds + " ms\n");
            richTextBox.AppendText("Nó\t Potencial\n");

            foreach (double item in results)
            {
                richTextBox.AppendText((count + 1).ToString() + " \t " + Math.Round(results[count, 0], 10).ToString() + "\n");
                count++;
            }

        }
    }
}

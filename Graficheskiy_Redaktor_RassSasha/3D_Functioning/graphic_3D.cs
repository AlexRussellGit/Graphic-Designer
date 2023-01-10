using Graficheskiy_Redaktor_RussAlex.Line_Functioning;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Graficheskiy_Redaktor_RussAlex._3D_Functioning
{
    class graphic_3D
    {
        private float[,] multiplyMatrix(float[,] _matrix1, float[,] _matrix2)
        {
            int matrix1_RowCount = 0, matrix1_ColumnCount = 0, matrix2_RowCount = 0, matrix2_ColumnCount = 0, matrixResult_RowCount = 0, matrixResult_ColumnCount = 0;
            float[,] _matrixResult = new float[4, 4];
            matrix1_RowCount = _matrix1.GetLength(0);
            matrix1_ColumnCount = _matrix1.GetLength(1);
            matrix2_RowCount = _matrix2.GetLength(0);
            matrix2_ColumnCount = _matrix2.GetLength(1);
            matrixResult_RowCount = _matrixResult.GetLength(0);
            matrixResult_ColumnCount = _matrixResult.GetLength(1);

            for (int i = 0; i < matrixResult_RowCount; i++)
            {
                for (int j = 0; j < matrixResult_ColumnCount; j++)
                {
                    _matrixResult[i, j] = 0;
                }
            }

            if (matrix1_ColumnCount == matrix2_RowCount)
            {
                for (int i = 0; i < matrix1_RowCount; i++)
                {
                    for (int j = 0; j < matrix2_ColumnCount; j++)
                    {
                        for (int r = 0; r < matrix1_ColumnCount; r++)
                        {
                            _matrixResult[i, j] = _matrixResult[i, j] + _matrix1[i, r] * _matrix2[r, j];
                        }
                    }

                }
            }
            else
            {
                MessageBox.Show("the first matix column count should be equal to second matrix row count", "Important");
            }

            return _matrixResult;            
        }
        public void drawAxis(int[,] _matrixScreen, Graphics gr)
        {
            Pen penDrawer = new Pen(new SolidBrush(Color.Red), 1);
            gr.DrawLine(penDrawer, _matrixScreen[0, 0], _matrixScreen[0, 1], _matrixScreen[1, 0], _matrixScreen[1, 1]);
            gr.DrawLine(penDrawer, _matrixScreen[2, 0], _matrixScreen[2, 1], _matrixScreen[3, 0], _matrixScreen[3, 1]);
            gr.DrawLine(penDrawer, _matrixScreen[4, 0], _matrixScreen[4, 1], _matrixScreen[5, 0], _matrixScreen[5, 1]);
        }

        public void writeAxisLetter(int[,] _matrixScreen, Graphics gr)
        {
            Font tf = new Font("Arial", 8);
            gr.DrawString("X", tf, Brushes.Red, _matrixScreen[1, 0], _matrixScreen[1, 1]);
            gr.DrawString("Y", tf, Brushes.Red, _matrixScreen[3, 0], _matrixScreen[3, 1]);
            gr.DrawString("Z", tf, Brushes.Red, _matrixScreen[5, 0], _matrixScreen[5, 1]);
        }

        // surface draw
        public void drawSurface(int[,] _surfaceCord, Graphics gr)
        {
            Pen penDraw = new Pen(new SolidBrush(Color.Purple), 1);
            gr.DrawLine(penDraw, _surfaceCord[0, 0], _surfaceCord[0, 1], _surfaceCord[1, 0], _surfaceCord[1, 1]);
            gr.DrawLine(penDraw, _surfaceCord[1, 0], _surfaceCord[1, 1], _surfaceCord[2, 0], _surfaceCord[2, 1]);
            gr.DrawLine(penDraw, _surfaceCord[2, 0], _surfaceCord[2, 1], _surfaceCord[3, 0], _surfaceCord[3, 1]);
            gr.DrawLine(penDraw, _surfaceCord[3, 0], _surfaceCord[3, 1], _surfaceCord[0, 0], _surfaceCord[0, 1]);
        }

        // color fill surface XOY, XOZ, YOZ
        public void fillSurfaceColor(int[,] planedCord, Graphics gr)
        {
            Point[] points = { new Point(planedCord[0, 0], planedCord[0, 1]), new Point(planedCord[1, 0], planedCord[1, 1]), new Point(planedCord[2, 0], planedCord[2, 1]), new Point(planedCord[3, 0], planedCord[3, 1]) };
            GraphicsPath graphicPath = new GraphicsPath();
            graphicPath.AddClosedCurve(points, 0.01f);
            SolidBrush transparentBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 100));
            gr.FillPath(transparentBrush, graphicPath);
        }

        // lins draw
        public void drawLines(int[,] _linesCord, bool _linesSelected , Graphics gr)
        {
            Pen penDraw = new Pen(new SolidBrush(Color.DarkCyan), 3);
            if (_linesSelected == true)
            {
                penDraw = new Pen(new SolidBrush(Color.DarkRed), 3);
            }
            gr.DrawLine(penDraw, _linesCord[0, 0], _linesCord[0, 1], _linesCord[1, 0], _linesCord[1, 1]);
        }

        public void writeLinesLetter(LineClass lineClass_List_Element, Graphics gr)
        {
            Font tf = new Font("Arial", 10);
            if (lineClass_List_Element.Selected == true)
            {
                float[,] displayCord = multiplyMatrix(lineClass_List_Element.MatrixLineStartCord, lineClass_List_Element.MatrixLineTransformation);

                gr.DrawString($"a:({Math.Round(displayCord[0, 0] * (1 / displayCord[0, 3]), 1)};{Math.Round(displayCord[0, 1] * (1 / displayCord[0, 3]), 1)};{Math.Round(displayCord[0, 2] * (1 / displayCord[0, 3]), 1)})", tf, Brushes.Black, lineClass_List_Element.MatrixLineDisplayCord[0, 0], lineClass_List_Element.MatrixLineDisplayCord[0, 1]);
                gr.DrawString($"b:({Math.Round(displayCord[1, 0] * (1 / displayCord[0, 3]), 1)};{Math.Round(displayCord[1, 1] * (1 / displayCord[0, 3]), 1)};{Math.Round(displayCord[1, 2] * (1 / displayCord[0, 3]), 1)})", tf, Brushes.Black, lineClass_List_Element.MatrixLineDisplayCord[1, 0], lineClass_List_Element.MatrixLineDisplayCord[1, 1]);
            }
        }
    }
}

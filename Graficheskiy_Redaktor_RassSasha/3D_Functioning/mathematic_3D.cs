using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graficheskiy_Redaktor_RussAlex._3D_Functioning
{
    class mathematic_3D
    {
        // multiplies 2 matrix into result matrix
        public void multiplyMatrix(float[,] _matrix1, float[,] _matrix2, float[,] _matrixResult)
        {
            int matrix1_RowCount = 0, matrix1_ColumnCount = 0, matrix2_RowCount = 0, matrix2_ColumnCount = 0, matrixResult_RowCount = 0, matrixResult_ColumnCount = 0;
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
        }

        // from homogeneous to decart coordinates, a transformed object
        public void toDecart(float[,] endCord, float[,] decartCord)
        {
            int matrix1_RowCount = endCord.GetLength(0);
            int matrix1_ColumnCount = endCord.GetLength(1);

            for (int i = 0; i < matrix1_RowCount; i++)
            {
                for (int j = 0; j < matrix1_ColumnCount; j++)
                {
                    if (endCord[i, 3] != 1)
                    {
                        if (endCord[i, 3] != 0)
                        {
                            decartCord[i, j] = endCord[i, j] / endCord[i, 3];
                        }
                        else
                        {
                            endCord[i, 3] = 0.01f;
                        }
                    }
                    else
                    {
                        decartCord[i, j] = endCord[i, j];
                    }
                }
            }
        }

        // from decart to screen coordinates & data frid view fill
        public void toDisplay(float[,] _matrixDecart, int[,] _matrixDisplay, int _displayWidth, int _displayHeight, float _mm_width, float _mm_height)
        {
            float matrixDecart_RowCount = _matrixDecart.GetLength(0);
            float matrixDecart_ColumnCount = _matrixDecart.GetLength(1);

            for (int i = 0; i < matrixDecart_RowCount; i++)
            {
                for (int j = 0; j < matrixDecart_ColumnCount; j++)
                {
                    if (j == 0) // y
                    {
                        _matrixDisplay[i, 0] = (int)(_displayWidth / 2 + (_mm_width * _matrixDecart[i, j]));
                    }
                    if (j == 1) // x
                    {
                        _matrixDisplay[i, 1] = (int)(_displayHeight / 2 - (_mm_height * _matrixDecart[i, j]));
                    }
                }
            }
            //fillMatrix(_matrixDisplay, dgvDisplay);
        }

        // fill data grid view with float data
        public void fillMatrix(float[,] _matrix, DataGridView _datagridview)
        {
            int matrixRowCount = 0, matrixColumnCount = 0;
            matrixRowCount = _matrix.GetLength(0);
            matrixColumnCount = _matrix.GetLength(1);
            _datagridview.RowCount = matrixRowCount;
            if (matrixRowCount > _datagridview.RowCount)
            {
                MessageBox.Show("Data grid Rows count should be greater " + "than matrix row count", "Important");
                return;
            }
            for (int i = 0; i < matrixRowCount; i++)
            {
                for (int j = 0; j < matrixColumnCount; j++)
                {
                    _datagridview.Rows[i].Cells[j].Value = _matrix[i, j];
                }
            }
        }

        // fill data grid view with int data
        public void fillMatrix(int[,] _matrix, DataGridView _datagridview)
        {
            int matrixRowCount = 0, matrixColumnCount = 0;
            matrixRowCount = _matrix.GetLength(0);
            matrixColumnCount = _matrix.GetLength(1);
            _datagridview.RowCount = matrixRowCount;
            if (matrixRowCount > _datagridview.RowCount)
            {
                MessageBox.Show("Data grid Rows count should be greater than matrix row count", "Important");
                return;
            }
            for (int i = 0; i < matrixRowCount; i++)
            {
                for (int j = 0; j < matrixColumnCount; j++)
                {
                    _datagridview.Rows[i].Cells[j].Value = _matrix[i, j];
                }
            }
        }

        // general transformed matrix
        public void countGeneralTransformedMatrix(Single[,] _matrixAxisTransformationRotation_Y, Single[,] _matrixAxisTransformationRotation_X, Single[,] _matrixOrtogonal_Z, Single[,] _matrixGeneralTransformation_XYZ, System.Windows.Forms.DataGridView _dgvGeneralMatrix)
        {
            float[,] _tempMatreix = new float[4, 4];
            multiplyMatrix(_matrixAxisTransformationRotation_Y, _matrixAxisTransformationRotation_X, _tempMatreix);
            multiplyMatrix(_tempMatreix, _matrixOrtogonal_Z, _matrixGeneralTransformation_XYZ);

            fillMatrix(_matrixGeneralTransformation_XYZ, _dgvGeneralMatrix);
        }

        public void matrixAxisTransformation(Single[,] _matrixAxisStartCord, Single[,] _matrixGeneralTransformation_XYZ, Single[,] _matrixAxisEndCord, System.Windows.Forms.DataGridView _dgvAxisFinal)
        {
            multiplyMatrix(_matrixAxisStartCord, _matrixGeneralTransformation_XYZ, _matrixAxisEndCord);
            fillMatrix(_matrixAxisEndCord, _dgvAxisFinal);
        }

        // surface tranformation
        public void matrixTransformationSurface(Single[,] _surfaceStart_XOY, Single[,] _surfaceStart_XOZ, Single[,] _surfaceStart_YOZ, Single[,] _matrixGeneralTransformation_XYZ, Single[,] _surfaceEnd_XOY, Single[,] _surfaceEnd_XOZ, Single[,] _surfaceEnd_YOZ)
        {
            multiplyMatrix(_surfaceStart_XOY, _matrixGeneralTransformation_XYZ, _surfaceEnd_XOY);
            multiplyMatrix(_surfaceStart_XOZ, _matrixGeneralTransformation_XYZ, _surfaceEnd_XOZ);
            multiplyMatrix(_surfaceStart_YOZ, _matrixGeneralTransformation_XYZ, _surfaceEnd_YOZ);
        }
        // line transformation
        public void matrixTransformationLine(Single [,] _matrixTransformation, Single[,] _matrixLineScale, Single[,] _matrixGeneralTransformation_XYZ_Gen, Single[,] _matrixGeneralTransformation_XYZ_Loc, Single[,] _matrixLineTransformation, Single[,] _matrixLineStartCord, Single[,] _matrixLineEndCord)
        {
            float[,] _tempMatreix1 = new float[4, 4];
            float[,] _tempMatreix2 = new float[4, 4];

            multiplyMatrix(_matrixLineTransformation, _matrixLineScale, _tempMatreix1);
            multiplyMatrix(_tempMatreix1, _matrixGeneralTransformation_XYZ_Loc, _matrixLineTransformation);
            multiplyMatrix(_matrixLineTransformation, _matrixGeneralTransformation_XYZ_Gen, _tempMatreix2);
            multiplyMatrix(_matrixLineStartCord, _tempMatreix2, _matrixLineEndCord);
        }
    }
}

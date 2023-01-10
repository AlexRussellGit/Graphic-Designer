using Graficheskiy_Redaktor_RussAlex._3D_Functioning;
using Graficheskiy_Redaktor_RussAlex.Line_Functioning;
using Graficheskiy_Redaktor_RussAlex.Storage_Functionong;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Graficheskiy_Redaktor_RussAlex
{
    public partial class Form1 : Form
    {

        public struct vektor3D
        {
            public float x, y, z;
        }

        public struct surfSides
        {
            public vektor3D[] pt;
            public int idp;
        }


        surfSides[] sideStart;
        surfSides[] sideEnd;

        float a1, a2, b1, b2;
        int M =30, N=30;

        float[,] matrixSurfaceDec;
        int[,] matrixSurfaceDispl;

        bool morfing = false;

        float MorfScale = 1;

        // objects for 3d mathematic and graphic working
        mathematic_3D math_3D = new mathematic_3D();
        graphic_3D graph_3D = new graphic_3D();

        // lines
        List<LineClass> lineList = new List<LineClass>();



        // matrix
        float[,] matrixAxisStartCord;
        float[,] matrixAxisEndCord;
        float[,] matrixAxisDecartCord;
        int[,] matrixAxisDisplayCord;
        float[,] matrixTransformation;

        // matrix transformation elements
        float /*mt_a, mt_b, mt_c, mt_p, mt_d, mt_e, mt_f, mt_q, mt_g, mt_h, mt_i, mt_r,*/ mt_l, mt_m, mt_n, mt_s;
        /// <summary>
        /// a  b  c  p
        /// d  e  f  q
        /// g  h  i  r
        /// l  m  n  s
        /// </summary>    

        // surface XOY, XOZ, YOZ
        float[,] surfaceStart_XOY;
        float[,] surfaceEnd_XOY;
        float[,] surfaceDecart_XOY;
        int[,] surfaceDisplay_XOY;

        float[,] surfaceStart_XOZ;
        float[,] surfaceEnd_XOZ;
        float[,] surfaceDecart_XOZ;
        int[,] surfaceDisplay_XOZ;
        
        float[,] surfaceStart_YOZ;
        float[,] surfaceEnd_YOZ;
        float[,] surfaceDecart_YOZ;
        int[,] surfaceDisplay_YOZ;

        // matrix transormation rotation
        float[,] matrixAxisTransformationRotation_Y;
        float[,] matrixAxisTransformationRotation_X;
        float[,] matrixOrtogonal_Z;
        float[,] matrixGeneralTransformation_XYZ;

        // display field parametres
        int displayWidth, displayHeight;
        int countAxis, mm_height, mm_width, h0h1, w0w1; // countAxis - scale segmentation, mm_Height & mm_width are by y & x; h0h1 & w0w1 - the distance from 0 to n
        float degreeState; // radean count
     
        int rotation_y; // saved rotation by y

        int rotation_x; // saves rotation by x
        
        // graphics parametrs
        Graphics graph; // for display

        Graphics gr; // memory, secondary graphic for canvas

        Bitmap fromBitmap; // canvas for main graph, so thats it didint blink
        
        Font f;
        
        // logic parametres
        bool iAmStarted = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            displayWidth = picDraw.Width;
            displayHeight = picDraw.Height;
            countAxis = 10;
            mm_width = displayWidth / (2 * (countAxis + 1));
            mm_height = displayHeight / (2 * (countAxis + 1));

            degreeState = (float)Math.PI/180;

            rotation_x = 0;
            rotation_y = 0;
            mt_l = 0;
            mt_m = 0;
            mt_n = 0;
            mt_s = 1;

            // transformation
            matrixTransformation = new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { mt_l, mt_m, mt_n, mt_s } };

            matrixAxisStartCord = new float[6, 4] { { -countAxis, 0, 0, 1 }, { countAxis, 0, 0, 1 }, { 0, -countAxis, 0, 1 }, { 0, countAxis, 0, 1 }, { 0, 0, -countAxis, 1 }, { 0, 0, countAxis, 1 } };



            matrixAxisTransformationRotation_Y = new float[4, 4] { { (float)Math.Cos(degreeState * rotation_y), 0, (float)Math.Sin(degreeState * rotation_y), 0 }, { 0, 1, 0, 0 }, { (float)Math.Sin(degreeState * rotation_y), 0, (float)Math.Cos(degreeState * rotation_y), 0 }, { 0, 0, 0, 1 } };

            matrixAxisTransformationRotation_X = new float[4, 4] { { 1, 0, 0, 0 }, { 0, (float)Math.Cos(degreeState * rotation_x), (float)Math.Sin(degreeState * rotation_x), 0 }, { 0, (float)Math.Sin(degreeState * rotation_x), (float)Math.Cos(degreeState * rotation_x), 0 }, { 0, 0, 0, 1 } };

            matrixOrtogonal_Z = new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 1 } };

            // redimention
            matrixGeneralTransformation_XYZ = new float[4, 4];
            matrixAxisEndCord = new float[6, 4];
            matrixAxisDecartCord = new float[6, 4];
            matrixAxisDisplayCord = new int[6, 4];

            // data-grid view
            dgvAxisStart.RowCount = 6;
            dgvAxisFinal.RowCount = 6;
            dgvAxisDecart.RowCount = 6;
            dgvAxisScreen.RowCount = 6;
            dgvTransformation.RowCount = 4;

            // surface initialization: XOY, XOZ, YOZ
            surfaceStart_XOY = new float[4, 4] { { -countAxis, countAxis, 0, 1 }, { countAxis, countAxis, 0, 1 }, { countAxis, -countAxis, 0, 1 }, { -countAxis, -countAxis, 0, 1 } };
            surfaceEnd_XOY = new float[4, 4];
            surfaceDecart_XOY = new float[4, 4];
            surfaceDisplay_XOY = new int[4, 4];

            surfaceStart_XOZ = new float[4, 4] { { -countAxis, 0, -countAxis, 1 }, { countAxis, 0, -countAxis, 1 }, { countAxis, 0, countAxis, 1 }, { -countAxis, 0, countAxis, 1 } };
            surfaceEnd_XOZ = new float[4, 4];
            surfaceDecart_XOZ = new float[4, 4];
            surfaceDisplay_XOZ = new int[4, 4];

            surfaceStart_YOZ = new float[4, 4] { { 0, -countAxis, -countAxis, 1 }, { 0, -countAxis, countAxis, 1 }, { 0, countAxis, countAxis, 1 }, { 0, countAxis, -countAxis, 1 } };
            surfaceEnd_YOZ = new float[4, 4];
            surfaceDecart_YOZ = new float[4, 4];
            surfaceDisplay_YOZ = new int[4, 4];
                       
            // matrix fill
            math_3D.fillMatrix(matrixAxisStartCord, dgvAxisStart);
            math_3D.fillMatrix(matrixTransformation, dgvTransformation);

            // graphics
            f = new Font("Arial", 10);
            graph = picDraw.CreateGraphics();
            fromBitmap = new Bitmap(displayWidth, displayHeight);
            gr = Graphics.FromImage(fromBitmap);
            picDraw.Image = fromBitmap;

            // morfing
            a1 = 0; a2 = (float)Math.PI; b1 = (float)(-Math.PI); b2 = (float)Math.PI;
            int M, N;
            
            // start
            startGeneral();

            // logical
            iAmStarted = true;
        }

        private void startGeneral()
        {
            mathematicsGeneral();
            graphicsGeneral();
        }

        private void mathematicsGeneral()
        {
            // axis
            math_3D.countGeneralTransformedMatrix(matrixAxisTransformationRotation_Y, matrixAxisTransformationRotation_X, matrixOrtogonal_Z, matrixGeneralTransformation_XYZ, dgvAxisScreen);
            math_3D.matrixAxisTransformation(matrixAxisStartCord, matrixGeneralTransformation_XYZ, matrixAxisEndCord, dgvAxisFinal);
            math_3D.toDecart(matrixAxisEndCord, matrixAxisDecartCord);
            math_3D.fillMatrix(matrixAxisDecartCord, dgvAxisDecart);
            math_3D.toDisplay(matrixAxisDecartCord, matrixAxisDisplayCord, displayWidth, displayHeight, mm_width, mm_height);
            math_3D.fillMatrix(matrixAxisDisplayCord, dgvAxisScreen);

            // lines
            lsvLines.Items.Clear();

            foreach (LineClass lineClass_List_Element in lineList)
            {
                math_3D.matrixTransformationLine(matrixTransformation, lineClass_List_Element.MatrixLineScale, matrixGeneralTransformation_XYZ, lineClass_List_Element.MatrixGeneralTransformation_XYZ, lineClass_List_Element.MatrixLineTransformation, lineClass_List_Element.MatrixLineStartCord, lineClass_List_Element.MatrixLineEndCord);
                math_3D.toDecart(lineClass_List_Element.MatrixLineEndCord, lineClass_List_Element.MatrixLineDecartCord);
                math_3D.toDisplay(lineClass_List_Element.MatrixLineDecartCord, lineClass_List_Element.MatrixLineDisplayCord, displayWidth, displayHeight, mm_width, mm_height);
                fillListView(lineList.IndexOf(lineClass_List_Element)+1, lineClass_List_Element, lsvLines);


                lineClass_List_Element.MatrixGeneralTransformation_XYZ = new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, mt_s } };
                lineClass_List_Element.MatrixLineScale = new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, mt_s } };
            }

            //fillList(lineList, );

            // surface
            if (chbDrawSurface.Checked == true)
            {
                math_3D.matrixTransformationSurface(surfaceStart_XOY, surfaceStart_XOZ, surfaceStart_YOZ, matrixGeneralTransformation_XYZ, surfaceEnd_XOY, surfaceEnd_XOZ, surfaceEnd_YOZ);
                
                chbFillSurface.Enabled = true;

                math_3D.toDecart(surfaceEnd_XOZ, surfaceDecart_XOZ);
                math_3D.toDisplay(surfaceDecart_XOZ, surfaceDisplay_XOZ, displayWidth, displayHeight, mm_width, mm_height);

                math_3D.toDecart(surfaceEnd_XOY, surfaceDecart_XOY);
                math_3D.toDisplay(surfaceDecart_XOY, surfaceDisplay_XOY, displayWidth, displayHeight, mm_width, mm_height);

                math_3D.toDecart(surfaceEnd_YOZ, surfaceDecart_YOZ);
                math_3D.toDisplay(surfaceDecart_YOZ, surfaceDisplay_YOZ, displayWidth, displayHeight, mm_width, mm_height);
            }
            else
            {
                chbFillSurface.Checked = false;
                chbFillSurface.Enabled = false;
            }

            if (morfing) 
            {
                createSurfacePointArrayMorfing();
            }
        }        

        private void graphicsGeneral()
        {
            // axis
            graph_3D.drawAxis(matrixAxisDisplayCord, gr);
            graph_3D.writeAxisLetter(matrixAxisDisplayCord, gr);

            // lines
            foreach(LineClass lineClass_List_Element in lineList)
            {
                graph_3D.drawLines(lineClass_List_Element.MatrixLineDisplayCord, lineClass_List_Element.Selected, gr);
                if (chbDisplaySelectedCoordinates.Checked)
                {
                    graph_3D.writeLinesLetter(lineClass_List_Element, gr);
                }
            }
            

            // surface
            if (chbDrawSurface.Checked == true)
            {
                graph_3D.drawSurface(surfaceDisplay_XOY, gr);
                graph_3D.drawSurface(surfaceDisplay_XOZ, gr);
                graph_3D.drawSurface(surfaceDisplay_YOZ, gr);
                chbFillSurface.Enabled = true;
                if (chbFillSurface.Checked == true)
                {
                    graph_3D.fillSurfaceColor(surfaceDisplay_XOY, gr);
                    graph_3D.fillSurfaceColor(surfaceDisplay_XOZ, gr);
                    graph_3D.fillSurfaceColor(surfaceDisplay_YOZ, gr);
                }
            }
            else
            {
                chbFillSurface.Checked = false;
                chbFillSurface.Enabled = false;
            }

            if (morfing)
            {
                drawSurface();
            }
        }

        // udate the grid view
        private void updatePictureBox()
        {
            gr.Clear(picDraw.BackColor);
            startGeneral();
            picDraw.Image = fromBitmap;
        }

        // update the elements and grid view
        private void globalUpdate()
        {
            displayWidth = picDraw.Width;
            displayHeight = picDraw.Height;

            mm_width = displayWidth / (2 * (countAxis + 1));
            mm_height = displayHeight / (2 * (countAxis + 1));

            graph = picDraw.CreateGraphics();
            fromBitmap = new Bitmap(displayWidth, displayHeight);
            gr = Graphics.FromImage(fromBitmap);
            picDraw.Image = fromBitmap;

            startGeneral();
        }

        private void fillListView(int _counter, LineClass _lineClass_List_Element, ListView _lsvLines)
        {
            ListViewItem listViewItem = new ListViewItem(_counter.ToString());
            listViewItem.SubItems.Add(_lineClass_List_Element.MatrixLineStartCord[0, 0].ToString());
            listViewItem.SubItems.Add(_lineClass_List_Element.MatrixLineStartCord[1, 0].ToString());
            listViewItem.SubItems.Add(_lineClass_List_Element.MatrixLineStartCord[0, 1].ToString());
            listViewItem.SubItems.Add(_lineClass_List_Element.MatrixLineStartCord[1, 1].ToString());
            listViewItem.SubItems.Add(_lineClass_List_Element.MatrixLineStartCord[0, 2].ToString());
            listViewItem.SubItems.Add(_lineClass_List_Element.MatrixLineStartCord[1, 2].ToString());
            _lsvLines.Items.Add(listViewItem);
        }

        // events
        private void TrbRotateY_ValueChanged(object sender, EventArgs e)
        {
            rotation_y = trbRotateY.Value;
            txtRotateY.Text = rotation_y.ToString();

            matrixAxisTransformationRotation_Y = new float[4, 4] { { (float)Math.Cos(degreeState * rotation_y), 0, (float)-Math.Sin(degreeState * rotation_y), 0 }, { 0, 1, 0, 0 }, { (float)Math.Sin(degreeState * rotation_y), 0, (float)Math.Cos(degreeState * rotation_y), 0 }, { 0, 0, 0, 1 } };

            updatePictureBox();
        }

        private void TrbRotateX_ValueChanged(object sender, EventArgs e)
        {
            rotation_x = trbRotateX.Value;
            txtRotateX.Text = rotation_x.ToString();

            matrixAxisTransformationRotation_X = new float[4, 4] { { 1, 0, 0, 0 }, { 0, (float)Math.Cos(degreeState * rotation_x), (float)Math.Sin(degreeState * rotation_x), 0 }, { 0, (float)Math.Sin(degreeState * rotation_x), (float)Math.Cos(degreeState * rotation_x), 0 }, { 0, 0, 0, 1 } };

            updatePictureBox();
        }

        private void TrbAxisCount_ValueChanged(object sender, EventArgs e)
        {
            if (iAmStarted)
            {
                countAxis = trbAxisCount.Value;
                txtAxisCount.Text = countAxis.ToString();

                globalUpdate();
            }
        }

        private void PicDraw_Resize(object sender, EventArgs e)
        {
            if (iAmStarted)
            {
                globalUpdate();
            }
        }

        private void ChbDrawSurface_CheckedChanged(object sender, EventArgs e)
        {
            updatePictureBox();
        }

        private void ChbFillSurface_CheckedChanged(object sender, EventArgs e)
        {
            updatePictureBox();
        }
        
        private void ChbDisplaySelectedCoordinates_CheckedChanged(object sender, EventArgs e)
        {
            updatePictureBox();
        }

        private void BtnAddNewLine_Click(object sender, EventArgs e)
        {
            if (iAmStarted)
            {
                if (CheckForInt(txtAddX1.Text, -10, 10) && CheckForInt(txtAddX2.Text, -10, 10) && CheckForInt(txtAddY1.Text, -10, 10) && CheckForInt(txtAddY2.Text, -10, 10) && CheckForInt(txtAddZ1.Text, -10, 10) && CheckForInt(txtAddZ2.Text, -10, 10))
                {
                    lineList.Add(new LineClass());

                    int lastListElement = lineList.Count - 1;

                    lineList[lastListElement].MatrixLineTransformation = new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, mt_s } }; ;
                    lineList[lastListElement].MatrixGeneralTransformation_XYZ = new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, mt_s } };
                    lineList[lastListElement].MatrixLineScale = new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, mt_s } };
                    lineList[lastListElement].MatrixLineStartCord = new float[2, 4] { { Convert.ToInt32(txtAddX1.Text), Convert.ToInt32(txtAddY1.Text), Convert.ToInt32(txtAddZ1.Text), 1 }, { Convert.ToInt32(txtAddX2.Text), Convert.ToInt32(txtAddY2.Text), Convert.ToInt32(txtAddZ2.Text), 1 } };
                    lineList[lastListElement].MatrixLineEndCord = new float[2, 4];
                    lineList[lastListElement].MatrixLineDecartCord = new float[2, 4];
                    lineList[lastListElement].MatrixLineDisplayCord = new int[2, 4];

                    globalUpdate();
                }
            }
        }

        private void BtnAddRandLine_Click(object sender, EventArgs e)
        {
            if (iAmStarted)
            {
                Random rand = new Random();
                lineList.Add(new LineClass());

                int lastListElement = lineList.Count - 1;

                lineList[lastListElement].MatrixLineTransformation = new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, mt_s } }; ;
                lineList[lastListElement].MatrixGeneralTransformation_XYZ = new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, mt_s } };
                lineList[lastListElement].MatrixLineScale = new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, mt_s } };
                lineList[lastListElement].MatrixLineStartCord = new float[2, 4] { { rand.Next(-10,11), rand.Next(-10, 11), rand.Next(-10, 11), 1 }, { rand.Next(-10, 11), rand.Next(-10, 11), rand.Next(-10, 11), 1 } };
                lineList[lastListElement].MatrixLineEndCord = new float[2, 4];
                lineList[lastListElement].MatrixLineDecartCord = new float[2, 4];
                lineList[lastListElement].MatrixLineDisplayCord = new int[2, 4];

                globalUpdate();
            }
        }

        private bool CheckForInt(string _string, int _minValue, int _maxValue)
        {
            try
            {
                int _value = Convert.ToInt32(_string);
                if (_value < _minValue || _value > _maxValue)
                {
                    MessageBox.Show("Inappropriate Value!\n(from -10 to 10)", "Error");
                    return false;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Inappropriate Value!", "Error");
                return false;
            }
            return true;
        }

        private void BtnClearLines_Click(object sender, EventArgs e)
        {
            if (iAmStarted)
            {
                lineList.Clear();
                morfing = false;
                globalUpdate();
            }
        }


        private void PicDraw_MouseClick(object sender, MouseEventArgs e)
        {
            if (lineList.Count != 0 && iAmStarted)
            {
                foreach(LineClass lineClass_List_Element in lineList)
                {
                    double x1 = lineClass_List_Element.MatrixLineDisplayCord[0, 0];
                    double x2 = lineClass_List_Element.MatrixLineDisplayCord[1, 0];

                    double y1 = lineClass_List_Element.MatrixLineDisplayCord[0, 1];
                    double y2 = lineClass_List_Element.MatrixLineDisplayCord[1, 1];


                    double deltaY = y2 - y1;
                    double deltaX = x2 - x1;

                    if (deltaY == 0)
                        deltaY = 0.01f;

                    double koeffK = deltaY / deltaX;
                    koeffK = Math.Round(koeffK, 2);
                    double koeffB = y1 - (koeffK * x1);

                    if (!(e.Y < (koeffK * e.X) + koeffB - 5) && !(e.Y > (koeffK * e.X) + koeffB + 5) && InRange(x1, x2, e.X) && InRange(y1, y2, e.Y)) 
                    {
                        lineClass_List_Element.Selected = !lineClass_List_Element.Selected;
                    }
                }
                globalUpdate();
            }
        }

        private bool InRange(double _coordinate_1, double _coordinate_2, double _coordinate_result)
        {
            if (_coordinate_2 >= _coordinate_1 && _coordinate_result >= _coordinate_1 - 1 && _coordinate_2 >= _coordinate_result - 1) 
                return true;
            else
            {
                if (_coordinate_1 >= _coordinate_2 && _coordinate_result >= _coordinate_2 - 1 && _coordinate_1 >= _coordinate_result - 1) 
                    return true;
            }

            return false;
        }

        private void BtnDeleteSelectedLines_Click(object sender, EventArgs e)
        {
            if (lineList.Count != 0 && iAmStarted)
            {
                foreach (LineClass lineClass_List_Element in new List<LineClass>(lineList))
                {
                    if (lineClass_List_Element.Selected)
                    {
                        lineList.Remove(lineClass_List_Element);
                    }
                }
                globalUpdate();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Storage.Save(saveFileDialog, lineList);
            globalUpdate();
        }
        
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            lineList = Storage.Load(openFileDialog, lineList);
            globalUpdate();
        }

        private void TrbMoveL_Scroll(object sender, EventArgs e)
        {
            if (lineList.Count != 0 && iAmStarted)
            {       
                foreach (LineClass lineClass_List_Element in lineList)
                {
                    if(lineClass_List_Element.Selected)
                    {
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[3, 0] += trbMoveL.Value;
                    }
                }
                updatePictureBox();
                trbMoveL.Value = 0;
            }
        }
        
        /// <summary>
        /// /////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(e.TabPage.Text.Equals("Morfing"))
            {
                lineList.Clear();
                morfing = true;
                globalUpdate();
            }
        }

        private void trbMorfing_Scroll(object sender, EventArgs e)
        {
            MorfScale = trbMorfing.Value;
            globalUpdate();
        }

        private void TrbMoveM_Scroll(object sender, EventArgs e)
        {
            if (lineList.Count != 0 && iAmStarted)
            {
                foreach (LineClass lineClass_List_Element in lineList)
                {
                    if (lineClass_List_Element.Selected)
                    {
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[3, 1] += trbMoveM.Value;
                    }
                }
                updatePictureBox();
                trbMoveM.Value = 0;
            }
        }

        private void TrbMoveN_Scroll(object sender, EventArgs e)
        {
            if (lineList.Count != 0 && iAmStarted)
            {
                foreach (LineClass lineClass_List_Element in lineList)
                {
                    if (lineClass_List_Element.Selected)
                    {
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[3, 2] += trbMoveN.Value;
                    }
                }
                updatePictureBox();
                trbMoveN.Value = 0;
            }
        }

        private void TrbScaleS_Scroll(object sender, EventArgs e)
        {
            if (lineList.Count != 0 && iAmStarted)
            {
                float km = 0.1f;
                if (trbScaleS.Value == -1)
                {
                    foreach (LineClass lineClass_List_Element in lineList)
                    {
                        if (lineClass_List_Element.Selected)
                        {
                            lineClass_List_Element.MatrixLineScale[3, 3] = km * 20;
                        }
                    }
                }
                if (trbScaleS.Value == 1)
                {
                    foreach (LineClass lineClass_List_Element in lineList)
                    {
                        if (lineClass_List_Element.Selected)
                        {
                            lineClass_List_Element.MatrixLineScale[3, 3] = km * 5;
                        }
                    }
                }
                updatePictureBox();
                trbScaleS.Value = 0;
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            if (lineList.Count != 0 && iAmStarted)
            {
                foreach (LineClass lineClass_List_Element in lineList)
                {
                    lineClass_List_Element.Selected = true;
                }
                updatePictureBox();
            }
        }

        private void BtnCancelSelect_Click(object sender, EventArgs e)
        {
            if (lineList.Count != 0 && iAmStarted)
            {
                foreach (LineClass lineClass_List_Element in lineList)
                {
                    lineClass_List_Element.Selected = false;
                }
                updatePictureBox();
            }
        }

        private void TrbRotateYFigure_Scroll(object sender, EventArgs e)
        {
            if (lineList.Count != 0 && iAmStarted)
            {
                foreach (LineClass lineClass_List_Element in lineList)
                {
                    if (lineClass_List_Element.Selected)
                    {
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[0, 0] = (float)Math.Cos(degreeState * trbRotateYFigure.Value);
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[0, 2] = (float)Math.Sin(degreeState * trbRotateYFigure.Value);
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[2, 0] = (float)Math.Sin(-(degreeState * trbRotateYFigure.Value));
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[2, 2] = (float)Math.Cos(degreeState * trbRotateYFigure.Value);
                    }
                }
                updatePictureBox();
                trbRotateYFigure.Value = 0;
            }
        }

        private void TrbRotateXFigure_Scroll(object sender, EventArgs e)
        {
            if (lineList.Count != 0 && iAmStarted)
            {
                foreach (LineClass lineClass_List_Element in lineList)
                {
                    if (lineClass_List_Element.Selected)
                    {
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[1, 1] = (float)Math.Cos(degreeState * trbRotateXFigure.Value);
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[1, 2] = (float)Math.Sin(-(degreeState * trbRotateXFigure.Value));
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[2, 1] = (float)Math.Sin(degreeState * trbRotateXFigure.Value);
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[2, 2] = (float)Math.Cos(degreeState * trbRotateXFigure.Value);
                    }
                }
                updatePictureBox();
                trbRotateXFigure.Value = 0;
            }
        }

        private void TrbRotateZFigure_Scroll(object sender, EventArgs e)
        {
            if (lineList.Count != 0 && iAmStarted)
            {
                foreach (LineClass lineClass_List_Element in lineList)
                {
                    if (lineClass_List_Element.Selected)
                    {
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[0, 0] = (float)Math.Cos(degreeState * trbRotateZFigure.Value);
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[0, 1] = (float)Math.Sin(-(degreeState * trbRotateZFigure.Value));
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[1, 0] = (float)Math.Sin(degreeState * trbRotateZFigure.Value);
                        lineClass_List_Element.MatrixGeneralTransformation_XYZ[1, 1] = (float)Math.Cos(degreeState * trbRotateZFigure.Value);
                    }
                }
                updatePictureBox();
                trbRotateZFigure.Value = 0;
            }
        }

        private void createSurfacePointArrayMorfing()
        {
            N = 30;
            M = 30;
            int i, j, q, MxN;
            float u, w, h1, h2;

            MxN = M * N;
            h1 = (a2 - a1) / N;
            h2 = (b2 - b1) / M;

            sideStart = new surfSides[MxN];
            sideEnd = new surfSides[MxN]; 

            float[,] temp = new float[4, 4];
            float[,] pattemp = new float[4, 4];

            for (i = 0; i < MxN; i++) 
            {
                sideStart[i].pt = new vektor3D[4];
                sideEnd[i].pt = new vektor3D[4];
            }
            q = -1;

            for (j = 0; j < N; j++)
            {
                for(i = 0; i < M; i++)
                {
                    q += 1;
                    //step1
                    u = a1 + h1 * (j + 0);
                    w = b1 + h2 * (i + 0);
                    vektor3D tp;
                    tp = calcilateSuefacePoint(u, w, 0);
                    temp[0, 0] = tp.x;
                    temp[0, 1] = tp.y;
                    temp[0, 2] = tp.z;
                    temp[0, 3] = 1;

                    sideEnd[q].pt[0] = tp;
                    //step2

                    u = a1 + h1 * (j + 0);
                    w = b1 + h2 * (i + 1);
                    tp = calcilateSuefacePoint(u, w, 0);
                    temp[1, 0] = tp.x;
                    temp[1, 1] = tp.y;
                    temp[1, 2] = tp.z;
                    temp[1, 3] = 1;
                    sideEnd[q].pt[1] = tp;
                    //step3

                    u = a1 + h1 * (j + 1);
                    w = b1 + h2 * (i + 1);
                    tp = calcilateSuefacePoint(u, w, 0);
                    temp[2, 0] = tp.x;
                    temp[2, 1] = tp.y;
                    temp[2, 2] = tp.z;
                    temp[2, 3] = 1;
                    sideEnd[q].pt[2] = tp;
                    //step4

                    u = a1 + h1 * (j + 1);
                    w = b1 + h2 * (i + 0);
                    tp = calcilateSuefacePoint(u, w, 0);
                    temp[3, 0] = tp.x;
                    temp[3, 1] = tp.y;
                    temp[3, 2] = tp.z;
                    temp[3, 3] = 1;
                    sideEnd[q].pt[3] = tp;

                    //transformation

                    trSurface(temp, pattemp);
                    //redimantion
                    matrixSurfaceDec = new float[4, 4];
                    //
                    math_3D.toDecart(pattemp, matrixSurfaceDec);
                    //
                    matrixSurfaceDispl = new int[4, 4];
                    math_3D.toDisplay(matrixSurfaceDec, matrixSurfaceDispl, displayWidth, displayHeight, mm_width, mm_height);

                    sideStart[q].pt[0].x = matrixSurfaceDispl[0, 0];
                    sideStart[q].pt[0].y = matrixSurfaceDispl[0, 1];
                    sideStart[q].pt[0].z = matrixSurfaceDispl[0, 2];

                    sideStart[q].pt[1].x = matrixSurfaceDispl[1, 0];
                    sideStart[q].pt[1].y = matrixSurfaceDispl[1, 1];
                    sideStart[q].pt[1].z = matrixSurfaceDispl[1, 2];

                    sideStart[q].pt[2].x = matrixSurfaceDispl[2, 0];
                    sideStart[q].pt[2].y = matrixSurfaceDispl[2, 1];
                    sideStart[q].pt[2].z = matrixSurfaceDispl[2, 2];

                    sideStart[q].pt[3].x = matrixSurfaceDispl[3, 0];
                    sideStart[q].pt[3].y = matrixSurfaceDispl[3, 1];
                    sideStart[q].pt[3].z = matrixSurfaceDispl[3, 2];

                }
            }

        }

        private vektor3D calcilateSuefacePoint(float u1, float w1, float v1)
        {
            vektor3D pat;

            float add_x = (float)(Math.Round(Math.Sin(u1) * Math.Cos(w1), 0, MidpointRounding.AwayFromZero) + ((-1) * (Math.Sin(u1) * Math.Cos(w1))));
            float add_y = (float)(Math.Round(Math.Sin(u1) * Math.Sin(w1), 0, MidpointRounding.AwayFromZero)  + ((-1) * (Math.Sin(u1) * Math.Sin(w1))));
            float add_z = (float)(Math.Round(Math.Cos(u1), 0, MidpointRounding.AwayFromZero) + ((-1) * (Math.Cos(u1))));

            pat.x = (float)((Math.Sin(u1) * Math.Cos(w1)) + ((MorfScale / 10) * add_x));
            pat.y = (float)((Math.Sin(u1) * Math.Sin(w1)) + ((MorfScale / 10) * add_y));
            pat.z = (float)((Math.Cos(u1)) + ((MorfScale / 10) * add_z));

            return pat;
        }

        private void trSurface(float[,] mf1, float[,] mf2)
        {
            math_3D.multiplyMatrix(mf1, matrixGeneralTransformation_XYZ, mf2);
        }

        private void drawSurface()
        {
            int i, L;
            Font f = new Font("Arial", 10);
            L = M * N;
            for (i = 0; i < L; i++)
            {
                gr.DrawLine(Pens.Red, sideStart[i].pt[0].x, sideStart[i].pt[0].y, sideStart[i].pt[1].x, sideStart[i].pt[1].y);
                gr.DrawLine(Pens.Red, sideStart[i].pt[1].x, sideStart[i].pt[1].y, sideStart[i].pt[2].x, sideStart[i].pt[2].y);
                gr.DrawLine(Pens.Red, sideStart[i].pt[2].x, sideStart[i].pt[2].y, sideStart[i].pt[3].x, sideStart[i].pt[3].y);
                gr.DrawLine(Pens.Red, sideStart[i].pt[3].x, sideStart[i].pt[3].y, sideStart[i].pt[0].x, sideStart[i].pt[0].y);
            }
        }

    }
}

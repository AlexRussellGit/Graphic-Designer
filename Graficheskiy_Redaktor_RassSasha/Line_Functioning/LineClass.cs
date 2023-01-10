using System;

namespace Graficheskiy_Redaktor_RussAlex.Line_Functioning
{
    [Serializable]
    public class LineClass
    {
        private float[,] matrixLineStartCord;
        private float[,] matrixLineEndCord;
        private float[,] matrixLineDecartCord;
        private int[,] matrixLineDisplayCord;
        private float[,] matrixGeneralTransformation_XYZ;
        private float[,] matrixLineTransformation;
        private float[,] matrixLineScale;
        private bool selected;

        public float[,] MatrixLineStartCord
        {
            set { matrixLineStartCord = value; }
            get { return matrixLineStartCord; }
        }

        public float[,] MatrixLineEndCord
        {
            set { matrixLineEndCord = value; }
            get { return matrixLineEndCord; }
        }

        public float[,] MatrixLineDecartCord
        {
            set { matrixLineDecartCord = value; }
            get { return matrixLineDecartCord; }
        }

        public int[,] MatrixLineDisplayCord
        {
            set { matrixLineDisplayCord = value; }
            get { return matrixLineDisplayCord; }
        }
        public float[,] MatrixGeneralTransformation_XYZ
        {
            set { matrixGeneralTransformation_XYZ = value; }
            get { return matrixGeneralTransformation_XYZ; }
        }

        public float[,] MatrixLineTransformation
        {
            set { matrixLineTransformation = value; }
            get { return matrixLineTransformation; }
        }

        public float[,] MatrixLineScale
        {
            set { matrixLineScale = value; }
            get { return matrixLineScale; }
        }
        
        public bool Selected
        {
            set { selected = value; }
            get { return selected; }
        }

        public LineClass(float[,] matrixLineStartCord, float[,] matrixLineEndCord, float[,] matrixLineDecartCord, int[,] matrixLineDisplayCord,float[,] matrixGeneralTransformation_XYZ, float[,] matrixLineTransformation, float[,] matrixLineScale)
        {
            this.matrixLineStartCord = matrixLineStartCord;
            this.matrixLineEndCord = matrixLineEndCord;
            this.matrixLineDecartCord = matrixLineDecartCord;
            this.matrixLineDisplayCord = matrixLineDisplayCord;
            this.matrixGeneralTransformation_XYZ = matrixGeneralTransformation_XYZ;
            this.matrixLineTransformation = matrixLineTransformation;
            this.matrixLineScale = matrixLineScale;
            this.selected = false;

        }

        public LineClass()
        {
            this.matrixLineStartCord = null;
            this.matrixLineEndCord = null;
            this.matrixLineDecartCord = null;
            this.matrixLineDisplayCord = null;
            this.matrixGeneralTransformation_XYZ = null;
            this.matrixLineTransformation = null;
            this.matrixLineScale = null;
            this.selected = false;
        }
    }
}

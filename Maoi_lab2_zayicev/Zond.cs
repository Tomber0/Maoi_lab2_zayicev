using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Maoi_lab2_zayicev
{
    class Zond
    {
        public ZondCrosses ZondCross { get; set; }
        public int YCoordinate { get; set; }
        public int XCoordinate { get; set; }


        //watrix that only shows lines of zonds
        public string[][] LinesMatrix { get; set; }
        public Matrix ZondMatrix { get; set; }
        public string[][] NormilizedMatrix { get; set; }

        public Image LocalZondImage { get; set; }
        public Zond(int x, int y,Matrix sourceMatrix) 
        {
            YCoordinate = y;
            XCoordinate = x;
            ZondMatrix = (Matrix)sourceMatrix.Clone();
            CreateZondMatrix();

        }

        private void CreateZondMatrix() 
        {
            LinesMatrix = Array.ConvertAll(ZondMatrix.HalftoneMatrix, a => (string[])a.Clone());//   ZondMatrix.HalftoneMatrix;
            for (int i = 0; i < ZondMatrix.Height; i++)
            {
                LinesMatrix[XCoordinate][i] = "-1";

            }
            for (int j = 0; j < ZondMatrix.Width; j++)
            {
                LinesMatrix[j][YCoordinate] = "-1";
            }



            ZondImage();
            //NormilizeZondMatrix(LinesMatrix);
        }
        private void ZondMarginImage() 
        {
            int x = 0;
            int y = 0;
            for (int i = 0 + 1; i < ZondMatrix.Height - 1; i++)
            {
                if ((LinesMatrix[XCoordinate][i - 1] == "-1") && (TransformToNumber(ZondMatrix.HalftoneMatrix[XCoordinate][i]) <= 150) && (LinesMatrix[XCoordinate][i + 1] == "-1"))
                    y++;
                while ((i < ZondMatrix.Height - 1) && ((TransformToNumber(ZondMatrix.HalftoneMatrix[XCoordinate][i]) <= 150)))
                {
                    i++;
                }
                // LinesMatrix[XCoordinate][i] = "-1";

            }
            for (int j = 0 + 1; j < ZondMatrix.Width - 1; j++)
            {
                if ((LinesMatrix[j - 1][YCoordinate] == "-1") && (TransformToNumber(ZondMatrix.HalftoneMatrix[j][YCoordinate]) <= 150) && (LinesMatrix[j + 1][YCoordinate] == "-1"))
                    x++;
                while ((j < ZondMatrix.Width - 1) && ((TransformToNumber(ZondMatrix.HalftoneMatrix[j][YCoordinate]) <= 150)))
                {
                    j++;
                }
                //LinesMatrix[j][YCoordinate] = "-1";
            }


            ZondCross = new ZondCrosses(x, y);





        }
        private void ZondImage() 
        {
            int x = 0;
            int y = 0;
            for (int i = 0+1; i < ZondMatrix.Height-1; i++)
            {
                if ((LinesMatrix[XCoordinate][i - 1] == "-1") && (TransformToNumber(ZondMatrix.HalftoneMatrix[XCoordinate][i]) <= 150) && (LinesMatrix[XCoordinate][i + 1] == "-1"))
                    y++;
                while ((i < ZondMatrix.Height - 1) &&((TransformToNumber(ZondMatrix.HalftoneMatrix[XCoordinate][i]) <= 150))) 
                {
                    i++;
                }
               // LinesMatrix[XCoordinate][i] = "-1";

            }
            for (int j = 0+1; j < ZondMatrix.Width-1; j++)
            {
                if ((LinesMatrix[j-1][YCoordinate] == "-1") && (TransformToNumber(ZondMatrix.HalftoneMatrix[j][YCoordinate]) <= 150) && (LinesMatrix[j + 1][YCoordinate] == "-1"))
                    x++;
                while ((j < ZondMatrix.Width - 1) && ((TransformToNumber(ZondMatrix.HalftoneMatrix[j][YCoordinate]) <= 150)))
                {
                    j++;
                }
                //LinesMatrix[j][YCoordinate] = "-1";
            }


            ZondCross = new ZondCrosses(x, y);

        }
        

        private int TransformToNumber(string strNum) 
        {
            string[] rGBstrings = strNum.Split(new char[] { ',', '\n' });
            int newColor = (Convert.ToInt32(rGBstrings[0]) +
                Convert.ToInt32(rGBstrings[1]) +
                Convert.ToInt32(rGBstrings[2])) / 3;

            return newColor;


        }

        public string[][] NormilizeZondMatrix(string[][] zondMatrix)
        {
            string[][] newMatrix = (string[][])zondMatrix.Clone();

            for (int i = 0; i < newMatrix.Length; i++)
            {
                for (int j = 0; j < newMatrix[i].Length; j++)
                {
                    if (newMatrix[i][j] == "-1") 
                    {
                        newMatrix[i][j] = "255,0,0";
                    }
                }
            }

            return newMatrix;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Maoi_lab2_zayicev
{

    /// <summary>
    /// Хранит матрицу изображения
    /// </summary>
    class Matrix:ICloneable
    {
        
        public Matrix(Image image)
        {
            LocalImage = image;
            this.Width = image.Width;
            this.Height = image.Height;
            SetUpMatrixes();
        }
        public Matrix( int width, int height)
        {
            this.Width = width;
            this.Height = height;
            CreateBlankImage();
            SetUpMatrixes();
        }
        private void CreateBlankImage() 
        {
            Bitmap imageBitmap = new Bitmap(this.Width, this.Height);
            Color[][] imagePixels = new Color[this.Width][];

            for (int i = 0; i < Width; i++)
            {
                imagePixels[i] = new Color[Height];
                for (int j = 0; j < Height; j++)
                {
                    imageBitmap.SetPixel(i, j,Color.White);
                    imagePixels[i][j] = Color.White;
                }
            }
            RawColorMatrix = imagePixels;
            LocalImage = imageBitmap;
        }
        private void SetUpMatrixes()
        {
            RawColorMatrix = GetPixelsFromImageToAnArray(this.LocalImage);
            RGBColorMatrix = ConvertArraysOfColorToArrayOfStrings(RawColorMatrix);
            HalftoneMatrix = ConvertRGBToHalftone(RGBColorMatrix);
            BinaryMatrix = TransformMatrixToBinary(HalftoneMatrix, 125);
        }
        public void TransformImageToHalfTone() 
        {
            RawColorMatrix = ConvertArraysOfStringToArrayOfColors(HalftoneMatrix);
            LocalImage = CreateImageFromColorArray();

        }
        public Image CreateImageFromColorArray() 
        {
            Bitmap imageBitmap = new Bitmap(this.Width, this.Height);

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    imageBitmap.SetPixel(i, j, RawColorMatrix[i][j]);
                }
            }
            return imageBitmap;

        }
        public Image LocalImage { get; set; }
        public Color[][] RawColorMatrix { get; set; }
        public int DarkLimit { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string[][] RGBColorMatrix { get; set; }
        public string[][] BinaryMatrix { get; set; }
        public string[][] HalftoneMatrix { get; set; }


        //string[][]
        public string[][] ConvertRGBToHalftone(string[][] rgbMatrix)
        {
            //1

            //2 in: basepixelstring
            // string[] rGBstrings = basePixelString.Split(new char[] { ',', '\n' });

            string[][] stringOfHalftoneMatrix = new string[Width][];
            for (int i = 0; i < Width; i++)
            {
                stringOfHalftoneMatrix[i] = new string[Height];
                for (int j = 0; j < Height; j++)
                {
                    string[] rGBstrings = rgbMatrix[i][j].Split(new char[] { ',', '\n' });

                    double[] colorsRGBDouble = new double[3];
                    for (int z = 0; z < rGBstrings.Length; z++)
                    {
                        colorsRGBDouble[z] = Convert.ToDouble(rGBstrings[z]);
                    }

                    double sumOfPixelsValues = 0;
                    foreach (var item in colorsRGBDouble)
                    {
                        sumOfPixelsValues += item / 3;
                        sumOfPixelsValues = Math.Round(sumOfPixelsValues, 0);
                    }
                    stringOfHalftoneMatrix[i][j] = $"{sumOfPixelsValues.ToString()},{sumOfPixelsValues.ToString()},{sumOfPixelsValues.ToString()}"
                    ;
                }
            }
            return stringOfHalftoneMatrix;
        }
        public Color[][] ConvertArraysOfStringToArrayOfColors(string[][] imageColorString)
        {
            Color[][] imageColorPixel = new Color[imageColorString.Length][];
            for (int i = 0; i < imageColorString.Length; i++)
            {
                imageColorPixel[i] = new Color[imageColorString[i].Length];
                for (int j = 0; j < imageColorString[i].Length; j++)
                {
                    //imageColorPixel[i][j] =  new Color();// $"{imageColorString[i][j].R},{imageColorString[i][j].G},{imageColorString[i][j].B}";
                    //imageColorPixel[i][j].R = imageColorString[i][j]
                    imageColorPixel[i][j] = GetColorFromString(imageColorString[i][j]);
                }
            }
            return imageColorPixel;
        }
        private Color GetColorFromString(string colorString)
        {
            string[] rGBstrings = colorString.Split(new char[] { ',', '\n' });
            Color color = Color.FromArgb(
                Convert.ToInt32(rGBstrings[0]),
                Convert.ToInt32(rGBstrings[1]),
                Convert.ToInt32(rGBstrings[2]));
            return color;
        }

        private string[][] TransformMatrixToBinary(string[][] halftoneMatrix , int limit) 
        {
            string[][] newBinarryMatrix =  Array.ConvertAll(halftoneMatrix, a => (string[])a.Clone());//   ZondMatrix.HalftoneMatrix;

            for (int i = 0; i < halftoneMatrix.Length; i++)
            {
                for (int j = 0; j < halftoneMatrix[i].Length; j++)
                {
                    if (Convert.ToInt32(newBinarryMatrix[i][j]) >= limit) 
                    {
                        newBinarryMatrix[i][j] = "255";
                    }
                    else
                        newBinarryMatrix[i][j] = "0";

                }
            }
            return newBinarryMatrix;
        }
        private Color[][] GetPixelsFromImageToAnArray(Image image)
        {
            int height = image.Height;
            int width = image.Width;


            Bitmap imageBitmap = new Bitmap(image);
            Color[][] imagePixels = new Color[width][];
            for (int i = 0; i < width; i++)
            {
                imagePixels[i] = new Color[height];
                for (int j = 0; j < height; j++)
                {
                    imagePixels[i][j] = imageBitmap.GetPixel(i, j);
                }
            }


            return imagePixels;
        }
        public string[][] ConvertArraysOfColorToArrayOfStrings(Color[][] imageColorPixels)
        {
            string[][] imageStringPixels = new string[imageColorPixels.Length][];
            for (int i = 0; i < imageColorPixels.Length; i++)
            {
                imageStringPixels[i] = new string[imageColorPixels[i].Length];
                for (int j = 0; j < imageColorPixels[i].Length; j++)
                {
                    imageStringPixels[i][j] = $"{imageColorPixels[i][j].R},{imageColorPixels[i][j].G},{imageColorPixels[i][j].B}";
                }
            }
            return imageStringPixels;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

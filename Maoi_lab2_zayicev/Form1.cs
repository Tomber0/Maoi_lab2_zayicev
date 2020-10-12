using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maoi_lab2_zayicev
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Matrix ImageMatrix;
        Matrix ImageZondsMatrix;

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "addImagelToolStripButton":
                    UploadImage();
                    break;
                case "openToolStripButton":
                    //открыть таблицу для записи
                    UploadImage();
                    break;

                default:
                    break;
            }
/*            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "ClickedItem", e.ClickedItem);
            messageBoxCS.AppendLine();
            MessageBox.Show(messageBoxCS.ToString(), "ItemClicked Event");
*/
        }

        private void UploadImage()
        {
            string filePath;
            Image image;
            DialogResult dialogRes = openFileDialog1.ShowDialog();
            if (CheckDialogResult(dialogRes))
            {
                filePath =
                openFileDialog1.FileName;

                image = new Bitmap(filePath);
                ImageMatrix = new Matrix(image);
                //transforms to halftone
                ImageMatrix.TransformImageToHalfTone();
                pictureBox1.Image = ImageMatrix.LocalImage;
                this.Text = $"Maoi_zayicev {openFileDialog1.SafeFileName}";
            }
        }
        private bool CheckDialogResult(DialogResult result)
        {
            switch (result)
            {
                case DialogResult.None:
                    return false;
                case DialogResult.OK:
                    return true;
                case DialogResult.Cancel:
                    return false;
                case DialogResult.Abort:
                    return false;
                case DialogResult.Retry:
                    return false;
                case DialogResult.Ignore:
                    return false;
                case DialogResult.Yes:
                    return true;
                case DialogResult.No:
                    return false;
                default:
                    return false;
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillPictureToWhiteBox();
        }
        private void FillPictureToWhiteBox() 
        {
            ImageZondsMatrix = new Matrix(pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = ImageZondsMatrix.LocalImage;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            if (IfImageExists())
            {
                MouseEventArgs me = (MouseEventArgs)e;
                Point coordinates = me.Location;
                //this.Text = $"Maoi_zayicev  x = {coordinates.X} Y = {coordinates.Y}";

                /*                ImageZondsMatrix = (Matrix)ImageMatrix.Clone();
                                Matrix matrix = new Matrix(ImageMatrix.LocalImage);
                                Zond zond = new Zond(coordinates.X, coordinates.Y, (Matrix)matrix.Clone());

                                ImageZondsMatrix.RGBColorMatrix = zond.NormilizeZondMatrix(zond.LinesMatrix);
                                ImageZondsMatrix.RawColorMatrix = ImageZondsMatrix.ConvertArraysOfStringToArrayOfColors(ImageZondsMatrix.RGBColorMatrix);
                */
                textBox2.Text = coordinates.X.ToString();
                textBox3.Text = coordinates.Y.ToString();

                pictureBox1.Image = SetupZondsWithCoordinates(coordinates.X, coordinates.Y,ImageMatrix.LocalImage);// ImageZondsMatrix.CreateImageFromColorArray();

            }
            else
            {
                MessageBox.Show("Отсутствует изображение!");
            }


        }

        private Image SetupZondsWithCoordinates(int x,int y, Image image) 
        {


            ImageZondsMatrix = (Matrix)ImageMatrix.Clone();
            Matrix matrix = new Matrix(image);
            Zond zond = new Zond(x, y, (Matrix)matrix.Clone());
            ImageZondsMatrix.RGBColorMatrix = zond.NormilizeZondMatrix(zond.LinesMatrix);
            ImageZondsMatrix.RawColorMatrix = ImageZondsMatrix.ConvertArraysOfStringToArrayOfColors(ImageZondsMatrix.RGBColorMatrix);

            return ImageZondsMatrix.CreateImageFromColorArray();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (IfImageExists())
            {
                pictureBox1.Image = SetupZondsWithCoordinates(ConvertTextBoxToInt(textBox2), ConvertTextBoxToInt(textBox3), ImageMatrix.LocalImage);
            }
            else 
            {
                MessageBox.Show("Отсутствует изображение!");
            }

        }
        private int ConvertTextBoxToInt(TextBox textBox) 
        {
            int convertedValue = 0;
            try
            {
                convertedValue = Convert.ToInt32(textBox.Text);
            }
            catch (Exception)
            {

            }
            return convertedValue;
            
        }
        private bool IfImageExists() 
        {
            return (pictureBox1.Image != null);
        
        }
    }
}

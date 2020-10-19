using Interlop = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel =  Microsoft.Office.Interop.Excel;

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
        static int imgCount = 0;
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
        List<RadioButton> buttonsPannel1;
        int CheckedPosition { get; set; } = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonsPannel1 = new List<RadioButton>();
            radioButton1.Checked = true;
            FillPictureToWhiteBox();
            buttonsPannel1.Add(radioButton1);
            buttonsPannel1.Add(radioButton2);
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Класс";
            dataGridView1.Columns[1].Name = "Вертикаль";
            dataGridView1.Columns[2].Name = "Горизонталь";
            dataGridView1.Columns[3].Name = "R";

        }
        private void FillPictureToWhiteBox() 
        {
            /*ImageZondsMatrix = new Matrix(pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = ImageZondsMatrix.LocalImage;
            */
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
            imgCount++;

            dataGridView1.Rows.Add($"{imgCount.ToString()}",$"{zond.ZondCross.HorisontalCrosses.ToString()}", $"{zond.ZondCross.VerticalCrosses.ToString()}" );//add finder

            //
            //zond - основные операции
            //

            switch (CheckedPosition)
            {
                case 0:
                    CheckLetter(zond);
                    break;
                case 1:
                    AddLetter(zond,textBox4.Text);

                    break;
                default:
                    break;
            }
            
/*           
 *           using (ImageContext dbContext = new ImageContext()) 
            {
                // -1 => test, 0 - normal; 1 - et
                ImageLetterModel imageLetterModel1 = new ImageLetterModel() { Letter = "R", Horizontal = 1, Vertical = 2,Type = -1 };
                dbContext.Image.Add(imageLetterModel1);
                dbContext.SaveChanges();
                
            }
*/
                return ImageZondsMatrix.CreateImageFromColorArray();
        }

        private void CheckLetter(Zond zond) 
        {

            using (ImageContext dbContext = new ImageContext())
            {
                var listOfStandarts = dbContext.Standart.ToList();
                double minDistance = double.MaxValue;
                char letter=' ';
                foreach (var item in listOfStandarts)
                {
                    double mTempDistance = FindEuclidian(zond.ZondCross.VerticalCrosses, item.Vertical, zond.ZondCross.HorisontalCrosses, item.Vertical);
                    if (minDistance > mTempDistance) 
                    {
                        minDistance = mTempDistance;
                        letter = item.Letter;
                    }

                }
                
                // -1 => test, 0 - normal; 1 - et
/*                ImageLetterModel imageLetterModel1 = new ImageLetterModel() { Letter = "R", Horizontal = 1, Vertical = 2, Type = -1 };
                dbContext.Standart.Find();
                dbContext.Image.Add(imageLetterModel1);
                dbContext.SaveChanges();
*/
            textBox1.Text = $"letter is '{letter}' \n Distance is {minDistance} ";
            }

        }

        private void UpdateListOfStandarts() 
        {
            //update list
            //get all
            //find averege of their h and v

            //foreach all elements  //find same class //update
            using (ImageContext dbContext = new ImageContext())
            {
                // -1 => test, 0 - normal; 1 - et
                var images = dbContext.Image.ToList();

                var avgImage = from image in images group image by image.Letter into imageGroup select new
                {
                    Letter = imageGroup.Key,
                    HorizontalAvg = imageGroup.Average(x => x.Horizontal),
                    VerticalAvg = imageGroup.Average(x => x.Vertical),

                };

                var avgList = avgImage.ToList();
                foreach (var item in avgList)
                {
                    var dbStandart = dbContext.Standart.FirstOrDefault(x => x.Letter == item.Letter);

                    if (dbStandart != null)
                    {
                        dbStandart.Vertical = item.VerticalAvg;
                        dbStandart.Horizontal = item.HorizontalAvg;
                    }
                    else 
                    {
                        dbContext.Standart.Add(new Models.StandartModel() {Horizontal = item.HorizontalAvg,Vertical = item.VerticalAvg });
                    }
                }
                dbContext.SaveChanges();
            }

        }
        private double FindEuclidian(double x1,double x2, double y1, double y2) 
        {
            return Math.Sqrt(Math.Pow(x1-x2,2.0) + Math.Pow(y1 - y2, 2.0));
        
        }
        private void AddLetter(Zond zond, string boxLetter) 
        {
            char letter = Convert.ToChar(boxLetter);
            if (boxLetter == null || boxLetter.Length <= 0 || boxLetter.Length > 1)
            {
                MessageBox.Show("Введите букву");
                textBox3.Text = "Проверьте ввод";
                return;
            }

            using (ImageContext dbContext = new ImageContext())
            {
                // -1 => test, 0 - normal; 1 - et
                ImageLetterModel imageLetterModel1 = new ImageLetterModel() { Letter = letter,Horizontal =zond.ZondCross.HorisontalCrosses,Vertical = zond.ZondCross.VerticalCrosses};
                dbContext.Image.Add(imageLetterModel1);


                dbContext.SaveChanges();

            }



            UpdateListOfStandarts();
        }


        /*     private string GetLetter(ZondCrosses crosses) { }
             private void AddLetterToDatabase(ZondCrosses crosses) { }
             private bool FindLetterRef(ZondCrosses crosses) 
             {
     */
        /*            Interlop.ApplicationClass app = new Interlop.ApplicationClass();
                 Interlop.Workbook myWorkBook = null;
                 Interlop.Worksheet mySheet = null;
                 myWorkBook = app.Workbooks.Open(@"C:\Users\Xiaomi\Desktop\a2\Img.xlsx");
                 mySheet = (Interlop.Worksheet)myWorkBook.Sheets["Img"];

                 Excel.Range dataRange = null;

                 int totalColumns = mySheet.UsedRange.Columns.Count;
                 int totalRows = mySheet.UsedRange.Rows.Count;

                 //Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                 app.Visible = true;
                 //true - буква найдена и существует и ее надо извлечь
                 //false - буквы нет  и ее надо добавить

                 List<string> output = new List<string>();
                 for (int row = 1; row < totalRows; row++) 
                 {
                     for (int col = 1; col < totalColumns; col++) 
                     {
                         dataRange = (Excel.Range)mySheet.Cells[row, col];
                         //Console.Write(String.Format(dataRange.Value2.ToString() + " "));
                         output.Add(dataRange.Value2.ToString());
                     }
                 }

                 crosses;
     */
        /*





             }*/
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

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;


        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            int i = 0;
            foreach (var item in buttonsPannel1)
            {
                if (item.Checked)
                {
                    CheckedPosition = i;
                }
                i++;
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "142";
            textBox3.Text = "260";

        }
    }
}

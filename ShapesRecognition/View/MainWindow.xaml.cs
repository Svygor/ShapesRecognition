using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using ShapesRecognition.Models;
using System.Drawing;

namespace ShapesRecognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NeuroNet NeuroNet { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public byte[] GetImage(string path)
        {
            Bitmap img = new Bitmap(path);
            byte[] pixels = new byte[img.Width * img.Height];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if (pixel.Name == "ffffffff")
                    {
                        pixels[i * img.Height + j] = 1;
                    }
                    else
                    {
                        pixels[i * img.Height + j] = 0;
                    }
                }
            }
            return pixels;
        }

        private void Learn_Click(object sender, RoutedEventArgs e)
        {
            List<byte[]> EducationalInputs = new List<byte[]>();
            List<double[]> EducationalOutputs = new List<double[]>();

            EducationalInputs.Add(GetImage("Images/circle.bmp"));
            EducationalInputs.Add(GetImage("Images/filled_circle.bmp"));
            EducationalInputs.Add(GetImage("Images/square.bmp"));
            EducationalInputs.Add(GetImage("Images/filled_square.bmp"));
            EducationalInputs.Add(GetImage("Images/triangle.bmp"));
            EducationalInputs.Add(GetImage("Images/filled_triangle.bmp"));
            for (int i = 0; i < 6; i++)
            {
                double[] mass = new double[6];
                for (int j = 0; j < mass.Length; j++)
                {
                    if (j == i)
                    {
                        mass[j] = 1;
                    }
                    else
                    {
                        mass[j] = 0;
                    }
                }
                EducationalOutputs.Add(mass);
            }


            NeuroNet = new NeuroNet();
            NeuroNet.Learn(EducationalInputs, EducationalOutputs, 100);

            MessageBox.Show("Learning Succesfully Completed");
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            NeuroNet.Think(GetImage("Images/test.bmp"));
            string result_test = "result_test: ";
            for (int i = 0; i < NeuroNet.CurrentResult.Length; i++)
            {
                result_test += " " + Math.Round(NeuroNet.CurrentResult[i],3);
            }

            NeuroNet.Think(GetImage("Images/test2.bmp"));
            string result_test2 = "\nresult_test2: ";
            for (int i = 0; i < NeuroNet.CurrentResult.Length; i++)
            {
                result_test2 += " " + Math.Round(NeuroNet.CurrentResult[i], 3);
            }

            NeuroNet.Think(GetImage("Images/circle.bmp"));
            string result_circle = "\nresult cirle: ";
            for (int i = 0; i < NeuroNet.CurrentResult.Length; i++)
            {
                result_circle += " " + Math.Round(NeuroNet.CurrentResult[i], 3);
            }

            NeuroNet.Think(GetImage("Images/filled_circle.bmp"));
            string result_filled_circle = "\nresult filled circle: ";
            for (int i = 0; i < NeuroNet.CurrentResult.Length; i++)
            {
                result_filled_circle += " " + Math.Round(NeuroNet.CurrentResult[i], 3);
            }

            NeuroNet.Think(GetImage("Images/square.bmp"));
            string result_square = "\nresult square: ";
            for (int i = 0; i < NeuroNet.CurrentResult.Length; i++)
            {
                result_square += " " + Math.Round(NeuroNet.CurrentResult[i], 3);
            }

            NeuroNet.Think(GetImage("Images/triangle.bmp"));
            string result_triangle = "\nresult triangle: ";
            for (int i = 0; i < NeuroNet.CurrentResult.Length; i++)
            {
                result_triangle += " " + Math.Round(NeuroNet.CurrentResult[i], 3);
            }

            string result = result_test + result_test2 + result_circle + result_filled_circle + result_square + result_triangle;

            MessageBox.Show(result);
        }
    }
}

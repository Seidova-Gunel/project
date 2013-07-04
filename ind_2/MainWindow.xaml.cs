using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MathParser;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO;
using System.IO.Log;
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections.ObjectModel;
using Gif.Components;

namespace ind_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Charts.Series.Clear();
        }

        int step=1;
        string funk,variable;
        string flag="textbox";
        string a1, b1, accurate1;
        string Path = @"D:\";
        double a, b, accurate,f_a,f_b;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public class ChartPoint
        {
            public double Value1 { get; set; }
            public double Value2 { get; set; }
        }

        private void FileMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HelpClick(object sender, RoutedEventArgs e)
        {

        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            string input;
            Parser p = new Parser();
            Parser pp = new Parser();
            a1 = t_a.Text;
            b1 = t_b.Text;
            accurate1 = t_accurate.Text;
            variable = t_variable.Text;
            funk = t_function.Text;
            input=funk;
            if ((funk!= "")|| (a1!= "") || (b1!= "")||(accurate1!=""))
            {
                string sPattern = @"(^(\+|\-){0,1}\d+$)|(^(\+|\-){0,1}\d+(\.|\,){1}\d+(\*10\^{0,1}\({0,1}(\+|\-){0,1}\d*\){0,1}){0,1}$)|(^\({0,1}(\+|\-){0,1}\d+\/{1}\d+\){0,1}(\*10\^{0,1}\({0,1}(\+|\-){0,1}\d*\){0,1}){0,1}$)|(^(\+|\-){0,1}\d+(\*10\^{0,1}\({0,1}(\+|\-){0,1}\d*\){0,1}){0,1}$)|(^(10\^{0,1}\({0,1}(\+|\-){0,1}\d*\){0,1}){1}$)";
                if (Regex.IsMatch(a1, sPattern)&&Regex.IsMatch(b1, sPattern)&&Regex.IsMatch(accurate1, sPattern))
                {
                    string p1=@"\.";
                    if(Regex.IsMatch(a1, p1))
                    {
                        a1 = Regex.Replace(a1, p1,",");
                    }
                    if(Regex.IsMatch(b1, p1))
                    {
                        b1 = Regex.Replace(b1, p1,",");
                    }
                    if(Regex.IsMatch(accurate1, p1))
                    {
                        accurate1 = Regex.Replace(accurate1, p1,",");
                    }
                    string p2=@"abs(.*)|acos(.*)|asin(.*)|atan(.*)|cos(.*)|cosh(.*)|floor(.*)|ln(.*)|log(.*)|sign(.*)|sin(.*)|sinh(.*)|qrt(.*)|tan(.*)|tanh(.*)";
                    if(Regex.IsMatch(variable, p2))
                    {
                        System.Windows.MessageBox.Show("Недопустимое имя переменной", "Ошибка!");
                    }
                    else
                    {
                         if(p.Evaluate(a1))
                         {
                             a=p.Result;
                         }
                         if (p.Evaluate(b1))
                         {
                             b = p.Result;
                         }
                         if (p.Evaluate(accurate1))
                         {
                             accurate = p.Result;
                         }
                        if(p.Evaluate(Regex.Replace(input, variable,"("+a.ToString()+")"))&& pp.Evaluate(Regex.Replace(input, variable,"("+b.ToString()+")")))
                        {
                            f_a=p.Result;
                            f_b=pp.Result;
                           
                                start.Visibility = Visibility.Collapsed;
                                Charts.Visibility = Visibility.Visible;
                                progressBar1.Visibility = Visibility.Visible;
                                if (s_picture.IsChecked == true)
                                {
                                    string path = "picture";
                                    if (!(Directory.Exists(path)))
                                    {
                                        Directory.CreateDirectory(path);
                                    }
                                    DirectoryInfo dirInfo = new DirectoryInfo(path);
                                    foreach (FileInfo file in dirInfo.GetFiles())
                                    {
                                        file.Delete();
                                    }
                                }
                                mt();
                                
                        


                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Введенная функция не может быть распознана. Проверьте правильность ввода.", "Ошибка!");
                        }
                    }


                }
                else
                {
                    System.Windows.MessageBox.Show("Данные введены некорректно (неизвестный формат)", "Ошибка!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Введите данные", "Ошибка!");
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void m_file(object sender, RoutedEventArgs e)
        {
           flag = "file";
           get_data();

        }

        private void m_TextBox(object sender, RoutedEventArgs e)
        {
            flag = "textbox";
            get_data();
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (radioButton_file.IsChecked == true)
            {
                flag = "file";
            }
            if (radioButton_TextBox.IsChecked == true)
            {
                flag = "textbox";
            }
            get_data();
        }


        public void mt()
        {
             double x,t=0,min,max, index,flag1=1;
             string input;
             Random random = new Random();
             Parser p = new Parser();
             Parser pp = new Parser();
           
             if (p.Evaluate(Regex.Replace(funk, variable, "(" + a.ToString() + ")")) && pp.Evaluate(Regex.Replace(funk, variable, "(" + b.ToString() + ")")))
             {
                 f_a = p.Result;
                 f_b = pp.Result;
             }
        
             min=a;
             max=b;
            if(f_a<f_b)
            {
                min=f_a;
                max=f_b;
            }
            else
            {
                min=f_b;
                max=f_a;
            }
             LineSeries NewChart = new LineSeries();
             ObservableCollection<ChartPoint> C1 = new ObservableCollection<ChartPoint> { };
             index = a;
             while (index <= b&&flag1==1)
             {
                 input = Regex.Replace(funk, variable, "(" + index.ToString() + ")");
                 if (p.Evaluate(input))
                 {
                     t = p.Result;
                 }
                 if (t * 0 == 0)
                 {
                     C1.Add(new ChartPoint { Value1 = t, Value2 = index });
                     if (t > max)
                         max = t;
                     if (t < min)
                         min = t;
                 }
                 else
                 {
                     System.Windows.MessageBox.Show("Разрыв функции", "Ошибка!");
                     progressBar1.Visibility = Visibility.Collapsed;
                     dispatcherTimer.Stop();
                     flag1 = 0;
                     Charts.Visibility = Visibility.Collapsed;
                 }
                 index = index + Math.Abs(b - a) / 100;
             }
             if (flag1 != 0)
             {
                 Y.Minimum = min - Math.Abs(b - a) / 50;
                 X.Minimum = min - Math.Abs(b - a) / 50;
                 Y.Maximum = max + Math.Abs(b - a) / 50;
                 X.Maximum = max + Math.Abs(b - a) / 50;
                 NewChart.ItemsSource = C1;
                 NewChart.DependentValuePath = "Value1";
                 NewChart.IndependentValuePath = "Value2";
                 Style style = new Style(typeof(LineDataPoint));
                 style.Setters.Add(new Setter(LineDataPoint.TemplateProperty, null));
                 Color background = Color.FromRgb((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255));
                 style.Setters.Add(new Setter(LineDataPoint.BackgroundProperty, new SolidColorBrush(background)));
                 NewChart.DataPointStyle = style;
                 Charts.Series.Add(NewChart);
                 progressBar1.UpdateLayout();
                




                 double x1;
                 double y;
                 double s = 0,l=0;
                 double fr = 0;
                 Random rnd1 = new Random();
                
                 for (int i = 0; i < accurate; i++) 
                 {
                     x1 = a+(b-a)*rnd1.NextDouble();
                    
                     y = 0 + rnd1.NextDouble() + rnd1.Next(Convert.ToInt32(Math.Abs(max)));
                     
         //если сгенерированное число попало в площадь, ограниченную кривой 
         //на данной координате значение интеграла увеличивается на 1
                     input = Regex.Replace(funk, variable, "(" + x1 + ")");
                      if (p.Evaluate(input))
                     {
                         t = p.Result;
                     }
                      if (Math.Abs(y) <t && x1 >= a && x1 <= b)
                      {
                          s = s + 1;
                      }
                 }

         //Делим полученное значение интеграла на общее число "выбросов" 
         //метода монте-карло 
        
                 MessageBox.Show("Точек = " + s.ToString());
               
                 s = Math.Abs(b - a) * Math.Abs(max) * s / accurate;
         
                 MessageBox.Show("Интеграл = "+s.ToString());
                 DriveInfo drv = new DriveInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                 if (drv.AvailableFreeSpace > 1048576)
                 {
                     if (s_picture.IsChecked == true)
                     {
                         if (Charts.Series[0] == null)
                         {
                             MessageBox.Show("there is nothing to export");
                         }
                         else
                         {
                             Rect bounds = VisualTreeHelper.GetDescendantBounds(Charts);

                             RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);

                             DrawingVisual isolatedVisual = new DrawingVisual();
                             using (DrawingContext drawing = isolatedVisual.RenderOpen())
                             {
                                 drawing.DrawRectangle(Brushes.White, null, new Rect(new Point(), bounds.Size)); // Optional Background
                                 drawing.DrawRectangle(new VisualBrush(Charts), null, new Rect(new Point(), bounds.Size));
                             }

                             renderBitmap.Render(isolatedVisual);

                             Microsoft.Win32.SaveFileDialog uloz_obr = new Microsoft.Win32.SaveFileDialog();
                             uloz_obr.FileName = "picture\\Graf" + step.ToString() + ".png";
                             uloz_obr.DefaultExt = "png";
                             string obr_cesta = uloz_obr.FileName;
                             using (FileStream outStream = new FileStream(obr_cesta, FileMode.Create))
                             {
                                 PngBitmapEncoder encoder = new PngBitmapEncoder();
                                 encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                                 encoder.Save(outStream);
                             }
                         }
                     }
                 }
                 else
                 {
                     l_s.Visibility = Visibility.Visible;
                     l_s.Content = "Сохранение изображений не возможно";
                 }
                 //step++;
             }
        }

        public void get_data()
        {
            if (flag == "file")
            {
                try
                {
                    string filename = "";
                    OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Текстовые файлы(*.txt)|*.txt" };
                    if (openFileDialog1.ShowDialog() != null)
                    {
                        filename = openFileDialog1.FileName;
                        FileStream stream = new FileStream(filename, FileMode.Open);
                        StreamReader reader = new StreamReader(stream);
                        t_a.Text = reader.ReadLine();
                        t_b.Text = reader.ReadLine();
                        t_accurate.Text = reader.ReadLine();
                        t_variable.Text = reader.ReadLine();
                        t_function.Text = reader.ReadLine();
                        t_function.ToolTip = "Уравнение, считаннное из файла";
                        stream.Close();
                        t_function.Visibility = Visibility.Visible;
                        canvas1.Visibility = Visibility.Visible;
                        start.Visibility = Visibility.Visible;
                        radioButton_file.Visibility = Visibility.Collapsed;
                        radioButton_TextBox.Visibility = Visibility.Collapsed;
                        next.Visibility = Visibility.Collapsed;
                        label_2.Visibility = Visibility.Visible;
                        s_picture.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Файл не выбран", "Ошибка!");
                    }
                }
                catch
                {
                }
            }
            else
            {
                t_function.Visibility = Visibility.Visible;
                canvas1.Visibility = Visibility.Visible;
                start.Visibility = Visibility.Visible;
                radioButton_file.Visibility = Visibility.Collapsed;
                radioButton_TextBox.Visibility = Visibility.Collapsed;
                next.Visibility = Visibility.Collapsed;
                label_1.Visibility = Visibility.Visible;
                s_picture.Visibility = Visibility.Visible;
            }
        }


        private void reset_Click(object sender, RoutedEventArgs e)
        {
            Charts.Series.Clear();
            dispatcherTimer.Stop();
            t_function.Visibility = Visibility.Collapsed;
            canvas1.Visibility = Visibility.Collapsed;
            start.Visibility = Visibility.Collapsed;
            radioButton_file.Visibility = Visibility.Visible;
            radioButton_TextBox.Visibility = Visibility.Visible;
            next.Visibility = Visibility.Visible;
            label_1.Visibility = Visibility.Collapsed;
            label_2.Visibility = Visibility.Collapsed;
            Charts.Visibility = Visibility.Collapsed;
            progressBar1.Visibility = Visibility.Collapsed;
            s_picture.Visibility = Visibility.Collapsed;
            l_s.Visibility = Visibility.Collapsed;
            flag = "textbox";
            t_function.Text = "";
            t_a.Text = "";
            t_b.Text = "";
            t_accurate.Text = "";
            t_variable.Text = "";
            funk = "";
            variable="";
            a1="";
            b1="";
            accurate1="";
            step = 1;
            string path = "picture";
            if (Directory.Exists(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
            }
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DynamicGradient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        System.Drawing.Bitmap ConvertBitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new System.Drawing.Bitmap(bitmap);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Choose an Image...",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tiff|All files|*.*"
            };
            if (ofd.ShowDialog() == true)
            {
                var bitmapImage = new BitmapImage(new Uri(ofd.FileName));
                image.Source = bitmapImage;
                var bitmap = ConvertBitmapImageToBitmap(bitmapImage);

                //Left
                var leftPixelColor = bitmap.GetPixel(2, bitmap.Height / 2);
                //Middle pixel
                var middlePixelColor = bitmap.GetPixel(bitmap.Width / 2, bitmap.Height / 2);
                //Right pixel
                var rightPixelColor = bitmap.GetPixel(bitmap.Width - 2, bitmap.Height / 2);

                var leftPixelColorBrush = new SolidColorBrush(Color.FromArgb(leftPixelColor.A, leftPixelColor.R, leftPixelColor.G, leftPixelColor.B));
                var middlePixelColorBrush = new SolidColorBrush(Color.FromArgb(middlePixelColor.A, middlePixelColor.R, middlePixelColor.G, middlePixelColor.B));
                var rightPixelColorBrush = new SolidColorBrush(Color.FromArgb(rightPixelColor.A, rightPixelColor.R, rightPixelColor.G, rightPixelColor.B));

                lblFilename.Foreground = middlePixelColorBrush;
                lblFilename.Effect = new DropShadowEffect {
                    BlurRadius = 5,
                    Color =  Colors.Black,
                    ShadowDepth = 5
                };
                label.Foreground = leftPixelColorBrush;
                button.Background = middlePixelColorBrush;

                var backgroundBrush = new LinearGradientBrush();
                backgroundBrush.GradientStops.Add(new GradientStop
                {
                    Color = leftPixelColorBrush.Color,
                    Offset = 0
                });
                backgroundBrush.GradientStops.Add(new GradientStop
                {
                    Color = middlePixelColorBrush.Color,
                    Offset = 0.4
                });

                backgroundBrush.GradientStops.Add(new GradientStop
                {
                    Color = middlePixelColorBrush.Color,
                    Offset = 0.6
                });
                backgroundBrush.GradientStops.Add(new GradientStop
                {
                    Color = rightPixelColorBrush.Color,
                    Offset = 1
                });
                backgroundBrush.Transform = new RotateTransform(90d);
                Background = backgroundBrush;

                lblFilename.Content = System.IO.Path.GetFileName(ofd.FileName);
                lblFilename.Focus();
            }
        }
    }
}

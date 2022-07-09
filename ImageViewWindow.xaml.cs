using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IllustViewer
{
    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageViewWindow : Window
    {
        private bool isWindowStyleBorderless = false;

        public ImageViewWindow(Size windowSize)
        {
            InitializeComponent();
            this.Width = windowSize.Width;
            this.Height = windowSize.Height;
            setWindowStyle(isWindowStyleBorderless);
        }

        public void TryLoadImage(string imagePath)
        {
            imageView.Source = new BitmapImage(new Uri(imagePath));
        }


        internal void TryLoadImage(BitmapSource src)
        {
            imageView.Source = src;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {}



        private void imageView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double deltaScale = 1;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                deltaScale *= 1.2;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                deltaScale *= 0.8;
            }
            imageView.Width = imageView.ActualWidth * deltaScale;
            imageView.Height = imageView.ActualHeight * deltaScale;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                isWindowStyleBorderless = !isWindowStyleBorderless;

                setWindowStyle(isWindowStyleBorderless);
            }
        }


        private void setWindowStyle(bool isBorederless)
        {
            if (isBorederless)
            {
                this.WindowStyle = WindowStyle.None;
            }
            else
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }

        private void ScrollViewer_MouseEnter(object sender, MouseEventArgs e)
        {
            scrollViewr.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewr.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        }

        private void ScrollViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            scrollViewr.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            scrollViewr.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }
    }
}

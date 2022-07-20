using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private StragePath? stragePathRef = null;
        private UppingFlag imageUnsavedFlag = new();

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

        public void SetStragePath(StragePath stragePath)
        {
            Debug.Assert(stragePathRef != stragePath);
            stragePathRef = stragePath;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        { }



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

        private void window_MouseEnter(object sender, MouseEventArgs e)
        {
            changeViewIfWindowForcused(true);
        }

        private void window_MouseLeave(object sender, MouseEventArgs e)
        {
            changeViewIfWindowForcused(false);
        }

        private void changeViewIfWindowForcused(bool isMouseInScreen)
        {
            if (isMouseInScreen)
            {
                scrollViewr.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                scrollViewr.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                saveButton.Visibility = canShowImageSaveButton() ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                scrollViewr.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                scrollViewr.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                saveButton.Visibility = Visibility.Collapsed;

            }
        }

        private bool canShowImageSaveButton()
        {
            return stragePathRef != null && imageUnsavedFlag.IsUp;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                trySaveImageSource();
                MessageBox.Show("画像を保存しました。");
            }
            catch (Exception)
            {
                MessageBox.Show("画像に失敗しました。");
            }
        }

        private void trySaveImageSource()
        {
            string fileName = getDateHashText(DateTime.Now) + ".png";
            string path = stragePathRef.MakeFilePath(fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                ImageSource source = imageView.Source;
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)source));
                encoder.Save(stream);
            }
            imageUnsavedFlag.MakeDown();
        }

        private string getDateHashText(DateTime dateTime)
        {
            const string format = "yyyy_MMddHH_mmss";
            string text = dateTime.ToString(format);
            return text;
        }
    }
}

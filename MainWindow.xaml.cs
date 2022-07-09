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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IllustViewer
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

        private void grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var name in fileNames)
                {
                    Debug.Print(name);

                    var window = new ImageViewWindow(new Size(this.Width, this.Height));
                    try
                    {
                        window.TryLoadImage(name);
                        window.Show();
                    }
                    catch (Exception ex)
                    {
                        window.Close();
                        MessageBox.Show(ex.Message);
                    }
                }
                
            }
        }


        private void loadFromClipboadButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ImageViewWindow(new Size(this.Width, this.Height));
            try
            {
                var src = GetClipboardBitmapDIB();
                if (src == null) throw new Exception("クリップボードから画像を読み取れませんでした。");
                window.TryLoadImage(src);
                window.Show();
            }
            catch (Exception ex)
            {
                window?.Close();
                MessageBox.Show(ex.Message);
            }
        }



        // 参考: https://gogowaten.hatenablog.com/entry/2019/11/12/201852
        // Autohr: gogowaten
        private BitmapSource GetClipboardBitmapDIB()
        {
            var data = Clipboard.GetDataObject();
            if (data == null) return null;

            var ms = data.GetData("DeviceIndependentBitmap") as System.IO.MemoryStream;
            if (ms == null) return null;

            //DeviceIndependentBitmapのbyte配列の15番目がbpp、
            //これが32未満ならBgr32へ変換、これでアルファの値が0でも255扱いになって表示される
            byte[] dib = ms.ToArray();
            if (dib[14] < 32)
            {
                return new FormatConvertedBitmap(Clipboard.GetImage(), PixelFormats.Bgr32, null, 0);
            }
            else
            {
                return Clipboard.GetImage();
            }
        }

    }
}

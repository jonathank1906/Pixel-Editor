using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace avaloniadefaultapp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowWritableBitmap();
        }

        private void ShowWritableBitmap()
        {
            int width = 10;
            int height = 10;
            var writableBitmap = new WriteableBitmap(new Avalonia.PixelSize(width, height), new Avalonia.Vector(96, 96), PixelFormat.Bgra8888);

            using (var fb = writableBitmap.Lock())
            {
                unsafe
                {
                    var buffer = (uint*)fb.Address;
                    for (int i = 0; i < width * height; i++)
                    {
                        buffer[i] = 0xFFFF0000; // Red color
                    }
                }
            }

            // Set the Image control size to match the bitmap
            imageControl.Width = width;
            imageControl.Height = height;

            // Optionally, set Stretch to Uniform to scale the image
             imageControl.Stretch = Avalonia.Media.Stretch.Uniform;

            // Set the bitmap as the source for the Image control
            imageControl.Source = writableBitmap;
        }

    }
}
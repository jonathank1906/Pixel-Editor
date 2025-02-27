using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace avaloniadefaultapp
{
    public partial class MainWindow : Window
    {
        private int scaleFactor = 25;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDefaultTab("smile.b2img.txt");
        }

        private void InitializeDefaultTab(string fileName)
{
    var context = new TabContext();

    imageDimensionsTextBlock = new TextBlock // Assign the TextBlock to the field
    {
        Name = "imageDimensionsTextBlock",
        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        Margin = new Thickness(5)
    };

    var imageControl = new Image
    {
        Width = 300,
        Height = 300,
        Stretch = Avalonia.Media.Stretch.None,
        Tag = context
    };
    imageControl.PointerPressed += ImageControl_PointerPressed;
    context.ImageControl = imageControl;

    var loadButton = new Button { Content = "Load", Margin = new Thickness(5), Tag = context };
    loadButton.Click += LoadButton_Click;

    var saveButton = new Button { Content = "Save As", Margin = new Thickness(5), Tag = context };
    saveButton.Click += SaveButton_Click;

    var flipVerticalButton = new Button { Content = "Flip Vertical", Margin = new Thickness(5), Tag = context };
    flipVerticalButton.Click += FlipVerticalButton_Click;

    var flipHorizontalButton = new Button { Content = "Flip Horizontal", Margin = new Thickness(5), Tag = context };
    flipHorizontalButton.Click += FlipHorizontalButton_Click;

    var tabItem = new TabItem
    {
        Header = Path.GetFileName(fileName),
        Content = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Vertical,
            Children =
            {
                imageDimensionsTextBlock, // Use the field here
                imageControl,
                new StackPanel
                {
                    Orientation = Avalonia.Layout.Orientation.Horizontal,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    Children =
                    {
                        loadButton,
                        saveButton,
                        flipVerticalButton,
                        flipHorizontalButton
                    }
                }
            }
        }
    };
    tabControl.Items.Clear();
    tabControl.Items.Add(tabItem);
    tabControl.SelectedItem = tabItem;

    ShowWritableBitmap(fileName, imageControl, context);
}

        private unsafe void DrawBorder(uint* buffer, int scaledWidth, int scaledHeight, uint borderColor, int borderThickness)
        {
            for (int y = 0; y < scaledHeight; y++)
            {
                for (int x = 0; x < scaledWidth; x++)
                {
                    if (x < borderThickness || x >= scaledWidth - borderThickness || y < borderThickness || y >= scaledHeight - borderThickness)
                    {
                        int borderIndex = y * scaledWidth + x;
                        buffer[borderIndex] = borderColor;
                    }
                }
            }
        }

        private void ShowWritableBitmap(string textFile, Image imageControl, TabContext context)
        {
            // Define border color and thickness
            uint borderColor = 0xFF000000; // Black color
            int borderThickness = 2;

            if (File.Exists(textFile))
            {
                string[] lines = File.ReadAllLines(textFile); // Read a text file line by line.
                if (lines.Length > 1)
                {
                    // Process the first line
                    string firstLine = lines[0];
                    string[] numbers = firstLine.Split(' ');
                    if (numbers.Length == 2)
                    {
                        int height = int.Parse(numbers[0]);
                        int width = int.Parse(numbers[1]);

                        Console.WriteLine($"Height: {height}, Width: {width}"); // Debug output

                        // Initialize the 2D array (matrix)
                        context.Matrix = new int[height, width];

                        // Process the second line as a matrix grid
                        string secondLine = lines[1];
                        for (int i = 0; i < height; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                int index = i * width + j;
                                if (index < secondLine.Length)
                                {
                                    context.Matrix[i, j] = secondLine[index] == '1' ? 1 : 0;
                                }
                                else
                                {
                                    context.Matrix[i, j] = 0; // Fill with 0 if index exceeds line length
                                }
                            }
                        }

                        // Create the original WriteableBitmap to display the image
                        var originalBitmap = new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96), PixelFormat.Bgra8888);

                        using (var fb = originalBitmap.Lock())
                        {
                            unsafe
                            {
                                var buffer = (uint*)fb.Address;

                                for (int i = 0; i < height; i++)
                                {
                                    for (int j = 0; j < width; j++)
                                    {
                                        int index = i * width + j;
                                        uint color = context.Matrix[i, j] == 1 ? 0xFF000000 : 0xFFFFFFFF; // 1 = black, 0 = white
                                        buffer[index] = color;
                                    }
                                }
                            }
                        }

                        // Create a scaled WriteableBitmap
                        int scaledWidth = width * scaleFactor;
                        int scaledHeight = height * scaleFactor;
                        context.ScaledBitmap = new WriteableBitmap(new PixelSize(scaledWidth, scaledHeight), new Vector(96, 96), PixelFormat.Bgra8888);

                        using (var fb = context.ScaledBitmap.Lock())
                        {
                            unsafe
                            {
                                var buffer = (uint*)fb.Address;

                                for (int i = 0; i < height; i++)
                                {
                                    for (int j = 0; j < width; j++)
                                    {
                                        int index = i * width + j;
                                        uint color = context.Matrix[i, j] == 1 ? 0xFF000000 : 0xFFFFFFFF; // 1 = black, 0 = white

                                        // Set the color for the scaled pixel size
                                        for (int y = 0; y < scaleFactor; y++)
                                        {
                                            for (int x = 0; x < scaleFactor; x++)
                                            {
                                                int scaledIndex = (i * scaleFactor + y) * scaledWidth + (j * scaleFactor + x);
                                                buffer[scaledIndex] = color;
                                            }
                                        }
                                    }
                                }

                                // Draw the border
                                DrawBorder(buffer, scaledWidth, scaledHeight, borderColor, borderThickness);
                            }
                        }

                        // Assign the scaledBitmap to the image control
                        imageControl.Source = context.ScaledBitmap;
                        Console.WriteLine("Bitmap assigned to image control."); // Debug output

                        // Update the dimensions text block
                        UpdateImageDimensions(width, height);
                    }
                }
                else
                {
                    Console.WriteLine("File does not contain enough lines."); // Debug output
                }
            }
            else
            {
                Console.WriteLine("File not found!"); // Debug output
            }
        }

        private void UpdateImageDimensions(int width, int height)
        {
            imageDimensionsTextBlock.Text = $"Size: {height}x{width}";
        }

        private async void LoadButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "Text Files", Extensions = { "txt" } }
                }
            };

            var result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                AddNewTab(result[0]);
            }
        }

        private async void SaveButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                DefaultExtension = "txt",
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "Text Files", Extensions = { "txt" } }
                }
            };

            var result = await saveFileDialog.ShowAsync(this);
            if (result != null)
            {
                var context = (TabContext)((Button)sender).Tag;
                using (var writer = new StreamWriter(result))
                {
                    writer.WriteLine($"{context.Matrix.GetLength(0)} {context.Matrix.GetLength(1)}");
                    for (int i = 0; i < context.Matrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < context.Matrix.GetLength(1); j++)
                        {
                            writer.Write(context.Matrix[i, j] == 1 ? '1' : '0');
                        }
                    }
                }
            }
        }

        private void ImageControl_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            var imageControl = sender as Image;
            var context = (TabContext)imageControl.Tag;
            var point = e.GetPosition(imageControl);
            int x = (int)(point.X / scaleFactor);
            int y = (int)(point.Y / scaleFactor);

            if (x >= 0 && x < context.Matrix.GetLength(1) && y >= 0 && y < context.Matrix.GetLength(0))
            {
                context.Matrix[y, x] = context.Matrix[y, x] == 1 ? 0 : 1; // Toggle the pixel color

                // Update the original bitmap
                using (var fb = context.ScaledBitmap.Lock())
                {
                    unsafe
                    {
                        var buffer = (uint*)fb.Address;
                        uint color = context.Matrix[y, x] == 1 ? 0xFF000000 : 0xFFFFFFFF; // 1 = black, 0 = white

                        // Set the color for the scaled pixel size
                        for (int py = 0; py < scaleFactor; py++)
                        {
                            for (int px = 0; px < scaleFactor; px++)
                            {
                                int scaledIndex = (y * scaleFactor + py) * context.ScaledBitmap.PixelSize.Width + (x * scaleFactor + px);
                                buffer[scaledIndex] = color;
                            }
                        }

                        // Redraw the border
                        DrawBorder(buffer, context.ScaledBitmap.PixelSize.Width, context.ScaledBitmap.PixelSize.Height, 0xFF000000, 2);
                    }
                }

                // Refresh the image control
                imageControl.InvalidateVisual();
            }
        }

        private void FlipHorizontalButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var context = (TabContext)((Button)sender).Tag;
            FlipImageVertically(context);
        }

        private void FlipVerticalButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var context = (TabContext)((Button)sender).Tag;
            FlipImageHorizontally(context);
        }

        private void FlipImageVertically(TabContext context)
        {
            int height = context.Matrix.GetLength(0);
            int width = context.Matrix.GetLength(1);
            int[,] flippedMatrix = new int[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    flippedMatrix[i, j] = context.Matrix[height - 1 - i, j];
                }
            }

            context.Matrix = flippedMatrix;
            UpdateBitmap(context);
        }

        private void FlipImageHorizontally(TabContext context)
        {
            int height = context.Matrix.GetLength(0);
            int width = context.Matrix.GetLength(1);
            int[,] flippedMatrix = new int[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    flippedMatrix[i, j] = context.Matrix[i, width - 1 - j];
                }
            }

            context.Matrix = flippedMatrix;
            UpdateBitmap(context);
        }

        private void UpdateBitmap(TabContext context)
        {
            int height = context.Matrix.GetLength(0);
            int width = context.Matrix.GetLength(1);

            // Create a scaled WriteableBitmap
            int scaledWidth = width * scaleFactor;
            int scaledHeight = height * scaleFactor;
            context.ScaledBitmap = new WriteableBitmap(new PixelSize(scaledWidth, scaledHeight), new Vector(96, 96), PixelFormat.Bgra8888);

            using (var fb = context.ScaledBitmap.Lock())
            {
                unsafe
                {
                    var buffer = (uint*)fb.Address;

                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            int index = i * width + j;
                            uint color = context.Matrix[i, j] == 1 ? 0xFF000000 : 0xFFFFFFFF; // 1 = black, 0 = white

                            // Set the color for the scaled pixel size
                            for (int y = 0; y < scaleFactor; y++)
                            {
                                for (int x = 0; x < scaleFactor; x++)
                                {
                                    int scaledIndex = (i * scaleFactor + y) * scaledWidth + (j * scaleFactor + x);
                                    buffer[scaledIndex] = color;
                                }
                            }
                        }
                    }

                    // Draw the border
                    DrawBorder(buffer, scaledWidth, scaledHeight, 0xFF000000, 2);
                }
            }

            // Assign the scaledBitmap to the image control
            context.ImageControl.Source = context.ScaledBitmap;
            Console.WriteLine("Bitmap updated and assigned to image control."); // Debug output
        }

        private void AddNewTab(string filePath)
        {
            var context = new TabContext();

            imageDimensionsTextBlock = new TextBlock // Assign the TextBlock to the field
            {
                Name = "imageDimensionsTextBlock",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Margin = new Thickness(5)
            };

            var imageControl = new Image
            {
                Width = 300,
                Height = 300,
                Stretch = Avalonia.Media.Stretch.None,
                Tag = context
            };
            imageControl.PointerPressed += ImageControl_PointerPressed;
            context.ImageControl = imageControl;

            var loadButton = new Button { Content = "Load", Margin = new Thickness(5), Tag = context };
            loadButton.Click += LoadButton_Click;

            var saveButton = new Button { Content = "Save As", Margin = new Thickness(5), Tag = context };
            saveButton.Click += SaveButton_Click;

            var flipVerticalButton = new Button { Content = "Flip Vertical", Margin = new Thickness(5), Tag = context };
            flipVerticalButton.Click += FlipVerticalButton_Click;

            var flipHorizontalButton = new Button { Content = "Flip Horizontal", Margin = new Thickness(5), Tag = context };
            flipHorizontalButton.Click += FlipHorizontalButton_Click;

            var tabItem = new TabItem
            {
                Header = Path.GetFileName(filePath),
                Content = new StackPanel
                {
                    Orientation = Avalonia.Layout.Orientation.Vertical,
                    Children =
                    {
                        imageDimensionsTextBlock, // Use the field here
                        imageControl,
                        new StackPanel
                        {
                            Orientation = Avalonia.Layout.Orientation.Horizontal,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            Children =
                            {
                                loadButton,
                                saveButton,
                                flipVerticalButton,
                                flipHorizontalButton
                            }
                        }
                    }
                }
            };

            tabControl.Items.Add(tabItem);
            tabControl.SelectedItem = tabItem;

            ShowWritableBitmap(filePath, imageControl, context);
        }
        private class TabContext
        {
            public int[,] Matrix { get; set; }
            public WriteableBitmap ScaledBitmap { get; set; }
            public Image ImageControl { get; set; }
        }
    }
}
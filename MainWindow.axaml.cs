using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace avaloniadefaultapp
{
    public partial class MainWindow : Window
    {
        private WriteableBitmap scaledBitmap;
        private int[,] matrix;
        private int scaleFactor = 25;

        public MainWindow()
        {
            InitializeComponent();
            ShowWritableBitmap();
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

        private void ShowWritableBitmap(string textFile = "smile.b2img.txt")
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
                        matrix = new int[height, width];

                        // Process the second line as a matrix grid
                        string secondLine = lines[1];
                        for (int i = 0; i < height; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                int index = i * width + j;
                                if (index < secondLine.Length)
                                {
                                    matrix[i, j] = secondLine[index] == '1' ? 1 : 0;
                                }
                                else
                                {
                                    matrix[i, j] = 0; // Fill with 0 if index exceeds line length
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
                                        uint color = matrix[i, j] == 1 ? 0xFF000000 : 0xFFFFFFFF; // 1 = black, 0 = white
                                        buffer[index] = color;
                                    }
                                }
                            }
                        }

                        // Create a scaled WriteableBitmap
                        int scaledWidth = width * scaleFactor;
                        int scaledHeight = height * scaleFactor;
                        scaledBitmap = new WriteableBitmap(new PixelSize(scaledWidth, scaledHeight), new Vector(96, 96), PixelFormat.Bgra8888);

                        using (var fb = scaledBitmap.Lock())
                        {
                            unsafe
                            {
                                var buffer = (uint*)fb.Address;

                                for (int i = 0; i < height; i++)
                                {
                                    for (int j = 0; j < width; j++)
                                    {
                                        int index = i * width + j;
                                        uint color = matrix[i, j] == 1 ? 0xFF000000 : 0xFFFFFFFF; // 1 = black, 0 = white

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
                        imageControl.Source = scaledBitmap;
                        Console.WriteLine("Bitmap assigned to image control."); // Debug output
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
                ShowWritableBitmap(result[0]);
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
                using (var writer = new StreamWriter(result))
                {
                    writer.WriteLine($"{matrix.GetLength(0)} {matrix.GetLength(1)}");
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            writer.Write(matrix[i, j] == 1 ? '1' : '0');
                        }
                    }
                }
            }
        }

        private void ImageControl_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            var point = e.GetPosition(imageControl);
            int x = (int)(point.X / scaleFactor);
            int y = (int)(point.Y / scaleFactor);

            if (x >= 0 && x < matrix.GetLength(1) && y >= 0 && y < matrix.GetLength(0))
            {
                matrix[y, x] = matrix[y, x] == 1 ? 0 : 1; // Toggle the pixel color

                // Update the original bitmap
                using (var fb = scaledBitmap.Lock())
                {
                    unsafe
                    {
                        var buffer = (uint*)fb.Address;
                        uint color = matrix[y, x] == 1 ? 0xFF000000 : 0xFFFFFFFF; // 1 = black, 0 = white

                        // Set the color for the scaled pixel size
                        for (int py = 0; py < scaleFactor; py++)
                        {
                            for (int px = 0; px < scaleFactor; px++)
                            {
                                int scaledIndex = (y * scaleFactor + py) * scaledBitmap.PixelSize.Width + (x * scaleFactor + px);
                                buffer[scaledIndex] = color;
                            }
                        }

                        // Redraw the border
                        DrawBorder(buffer, scaledBitmap.PixelSize.Width, scaledBitmap.PixelSize.Height, 0xFF000000, 2);
                    }
                }

                // Refresh the image control
                imageControl.InvalidateVisual();
            }
        }
    }
}
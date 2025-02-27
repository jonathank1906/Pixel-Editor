using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.IO;

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
            string textFile = "smile.b2img.txt"; // File path
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
                        int[,] matrix = new int[height, width];

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
                        int scaleFactor = 10;
                        int scaledWidth = width * scaleFactor;
                        int scaledHeight = height * scaleFactor;
                        var scaledBitmap = new WriteableBitmap(new PixelSize(scaledWidth, scaledHeight), new Vector(96, 96), PixelFormat.Bgra8888);

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
    }
}
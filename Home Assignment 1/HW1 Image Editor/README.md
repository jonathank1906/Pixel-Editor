Genius Engineers Group:
The provided code is a complete implementation of a pixel editor application using the Avalonia UI framework.

I. Main Components
-MainWindow Class:
Initialization: The MainWindow class initializes the UI components and sets up the default tab with a sample image file (smile.b2img.txt).
Image Display: It uses a WriteableBitmap to display images and allows users to interact with the image (e.g., flipping and saving).
Event Handling: Various event handlers are implemented for loading, saving, and manipulating the image.

-TabContext Class:
This class holds the state for each tab, such as the image matrix, scaled bitmap, image control, file path, and scale factor.

-UI Elements:
TabControl: This manages multiple tabs and each containing an image and control buttons.
Image Control: Displays the image using a WriteableBitmap.
Buttons: Provide functionality for loading, saving, flipping and other operations.

-Image Manipulation:
Loading and Saving: The application allows loading images from .txt files and saving them back.
Flipping: Functions to flip the image vertically and horizontally.
Pointer Interaction: Users can click on the image to toggle pixel colors.

-File Handling:
LoadButton_Click: Opens a file dialog to load a new image file.
SaveButtonAs_Click and SaveButton_Click: Save the current image to a file, with options to overwrite or save as a new file.

-Scaling and Border Drawing:
The application scales the image based on its dimensions and draws a border around the scaled image.


II. Contributors
Azzam:
UI Design and Layout: Azzam was responsible for designing the user interface and laying out the main window. He also implemented the event handlers for button clicks, such as LoadButton_Click, SaveButtonAs_Click, and SaveButton_Click.
Debugging and Testing: Azzam also focused on testing the application and ensuring errors are fixed, and the app works properly.

Jonathan:
Image Manipulation: Jonathan implemented the functionality to flip images vertically and horizontally. He wrote the FlipImageVertically and FlipImageHorizontally methods, which update the image matrix and refresh the display.
Pointer Interaction: He added the ability to toggle pixel colors by clicking on the image. This involves updating the image matrix and redrawing the scaled bitmap.
Bitmap Handling: He worked on creating and updating the WriteableBitmap for displaying the image, including scaling and border drawing.
File Dialogs: He integrated file dialogs for loading and saving image files, ensuring the application can read from and write to b2img.txt files.
b2img.txt files: He contribute in adding the b2img.txt to the codebase, making it easier for the group to continue working with the application

Daniils:
Tab Management: He implemented the logic for adding new tabs and managing multiple images. He wrote the AddNewTab method, which initializes a new tab with the loaded image and sets up the necessary controls.
State Management: He created the TabContext class to manage the state of each tab, including the image matrix, scaled bitmap, and file path.


III. Guide
Loading an Image:
Click the "Load" button to open a file dialog and select a .b2img.txt file containing image data.
The application will display the image in a new tab.
Editing an Image:
Click on the image to toggle pixel colors.
Use the "Flip Vertical" and "Flip Horizontal" buttons to manipulate the image.
Saving an Image:
Click the "Save" button to overwrite the current file or "Save As" to save the image to a new file.

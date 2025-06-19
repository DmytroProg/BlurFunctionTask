# Blur Function Task

In this task you have to implement method `BlurPicture`.

By running the app you will see a simple pixel graphics designer.
You need to implement the feature to blur the pixels picture in `GraphicsCore/GraphicsFunctions`, where you have to implement method `BlurPicture()`.

For pixel arts the most commonly used way to blur picture is to use 'Guassian blur 3x3'. Use this matrix to apply it for the blur.

In input you will get a 2-dimentional array with a record `ColorData` that has 3 properties representing RGB color scheme.
Properly apply 'Guassian blur 3x3' on the colors in the array to return an array with blured colors.

If you run Unit Tests you will see if you implemented the blur in a correct way.
This task is considered complete only if all Unit Tests are passed.

After that you will be able to play with blur in your pixel art designer!

Link to blur matrixes: https://en.wikipedia.org/wiki/Kernel_(image_processing)

# PalateMinimizer
A small program to change an Image to only have a specified palate

Usage:
 PalateMinimizer input-path output-path color-0 color-1 ...
 The input image has to be in the rgba32 pixel format.

To run: Clone the repository, open it and type _dotnet run [Program-Arguments]_.

Requierments: dotnet sdk 6.0

It works by looping over each pixel, finding the two colors in the palate closest to the current pixels color and than randomly choosing one of them, with the one closer to the actual color being more likely to be chosen.

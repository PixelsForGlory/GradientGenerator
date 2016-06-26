# Gradient Generator Library
Library to create gradients for use in Unity3D.  

Build status:

[![Build status](https://ci.appveyor.com/api/projects/status/hmdj9iroob2erp1r/branch/master?svg=true)](https://ci.appveyor.com/project/LlamaBot/gradientgenerator/branch/master)

## Building
The solution has a reference set for `UnityEngine.dll`, but the path for this assembly is not set. To build the library, a reference path must be set to the Managed directory (Default is C:\Program Files\Unity\Editor\Data\Managed).

## Installation
From a build or downloaded release, copy the `GradientGenerator.dll` to `[PROJECT DIR]\Assets\Plugins`

If using the Pixels for Glory NuGet repository at http://pixelsforglory.azurewebsites.net/nuget, install the `PixelsForGlory.GradientGenerator` package into your own class library project or install the `PixelsForGlory.Unity3D.GradientGenerator` package into a Unity3D project.

## Usage
There are four types of gradients that can be generated:

1. **Square Gradient**
   
   **Code:**
    
        using PixelsForGlory.GradientGenerator;
        ...
       
        var gradient = new SquareGradient(256, 256);
        using(var image = new Bitmap(@".\image.png"))
        {
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    int result = (byte)(255 * gradient.Generate(x, y));
                    var color = Color.FromArgb(255, result, result, result);
                    image.SetPixel(x, y, color);
                }
            }
        }
      
   **Result:**
      
      ![Square Gradient](./GradientGeneratorTest/OriginalImages/SquareGradient.png?raw=true "Square Gradient")

2. **Ramp Gradient**

   **Code:**
    
        using PixelsForGlory.GradientGenerator;
        ...
       
        var gradient = new RampGradient(256, 256);
        using(var image = new Bitmap(@".\image.png"))
        {
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    int result = (byte)(255 * gradient.Generate(x, y));
                    var color = Color.FromArgb(255, result, result, result);
                    image.SetPixel(x, y, color);
                }
            }
        }
   
   **Result:**
       
      ![Ramp Gradient](./GradientGeneratorTest/OriginalImages/RampGradient.png?raw=true "Ramp Gradient")

3. **Spiral Gradient**
   
   * divisions: The spiral and be divided into different divisions to create more interesting gradients.  There must be more than 2 divisions in the list and the points must be in ascending order (from 0 to 2 * PI).  The example below shows proper use.

   a. **Spiral Gradient With No Divisions**
  
   **Code:**
       
        using PixelsForGlory.GradientGenerator;
        ...
   
        var gradient = new SpiralGradient(128, 128, 256, 256);
        using(var image = new Bitmap(@".\image.png"))
        {
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    int result = (byte)(255 * gradient.Generate(x, y));
                    var color = Color.FromArgb(255, result, result, result);
                    image.SetPixel(x, y, color);
                }
            }
        }

   **Result:**
      
      ![Spiral Gradient](./GradientGeneratorTest/OriginalImages/SpiralGradient128_128.png?raw=true "Spiral Gradient")
      
   b. **Spiral Gradient With Divisions**
   
   **Code:**
       
        using PixelsForGlory.GradientGenerator;
        ...
   
        var gradient = new SpiralGradient(128, 128, 256, 256,
           new List<SpiralGradient.SpiralGradientDivision>()
           {
              new SpiralGradient.SpiralGradientDivision { Value = 0f, Point = 0f },
              new SpiralGradient.SpiralGradientDivision { Value = 1f, Point =     (2f * Mathf.PI) / 8 },
              new SpiralGradient.SpiralGradientDivision { Value = 0f, Point = 2 * (2f * Mathf.PI) / 8 },
              new SpiralGradient.SpiralGradientDivision { Value = 1f, Point = 3 * (2f * Mathf.PI) / 8 },
              new SpiralGradient.SpiralGradientDivision { Value = 0f, Point = 4 * (2f * Mathf.PI) / 8 },
              new SpiralGradient.SpiralGradientDivision { Value = 1f, Point = 5 * (2f * Mathf.PI) / 8 },
              new SpiralGradient.SpiralGradientDivision { Value = 0f, Point = 6 * (2f * Mathf.PI) / 8 },
              new SpiralGradient.SpiralGradientDivision { Value = 1f, Point = 7 * (2f * Mathf.PI) / 8 },
              new SpiralGradient.SpiralGradientDivision { Value = 0f, Point = 8 * (2f * Mathf.PI) / 8 }
           });
        using(var image = new Bitmap(@".\image.png"))
        {
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    int result = (byte)(255 * gradient.Generate(x, y));
                    var color = Color.FromArgb(255, result, result, result);
                    image.SetPixel(x, y, color);
                }
            }
        }

   **Result:**
      
      ![Spiral Gradient](./GradientGeneratorTest/OriginalImages/SpiralGradientWithDivisions128_128.png?raw=true "Spiral Gradient")
      
3. **Radial Gradient**
   
   There are a couple of options for creating a spiral gradient.  
   * clampToEdge: Instead of extending the gradient outside of the bounds of the image, this option scales the radiuses down to the bounds of the image .
   * divisions: The spiral and be divided into different divisions to create more interesting gradients.  There must be more than 2 divisions in the list and the points must be in ascending order (from 0,0 to LengthX/2, LengthY/2).  The example below shows proper use.

   a. **Radial Gradient With No Divisions**
  
   **Code:**
       
        using PixelsForGlory.GradientGenerator;
        ...
   
        var gradient = new RadialGradient(128, 128, 256, 256, false);
        using(var image = new Bitmap(@".\image.png"))
        {
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    int result = (byte)(255 * gradient.Generate(x, y));
                    var color = Color.FromArgb(255, result, result, result);
                    image.SetPixel(x, y, color);
                }
            }
        }

   **Result:**
      
      ![Radial Gradient](./GradientGeneratorTest/OriginalImages/RadialGradient128_128.png?raw=true "Radial Gradient")
      
   b. Radial Gradient With Divisions
   
   **Code:**
       
        using PixelsForGlory.GradientGenerator;
        ...
   
        var gradient = new RadialGradient(128, 128, 256, 256, true,
           new List<RadialGradient.RadialGradientDivision>()
           {
              new RadialGradient.RadialGradientDivision { Value = 0f, Point = Vector2.zero },
              new RadialGradient.RadialGradientDivision { Value = 0f, Point = new Vector2(128.0f - (128f * 0.75f), 128.0f - (128.0f * 0.75f)) },
              new RadialGradient.RadialGradientDivision { Value = 0.9f, Point = new Vector2(128.0f - (128.0f * 0.25f), 128.0f - (128.0f * 0.25f)) },
              new RadialGradient.RadialGradientDivision { Value = 1f, Point = new Vector2(128.0f, 128.0f) }
          });
        using(var image = new Bitmap(@".\image.png"))
        {
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    int result = (byte)(255 * gradient.Generate(x, y));
                    var color = Color.FromArgb(255, result, result, result);
                    image.SetPixel(x, y, color);
                }
            }
        }

   **Result:**
      
      ![Radial Gradient](./GradientGeneratorTest/OriginalImages/RadialGradientWithDivisions128_128.png?raw=true "Radial Gradient")

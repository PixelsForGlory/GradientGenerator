# Gradient Generator Library
Library to create gradients for use in Unity3D.  

Build status:
TBD

## Building
The solution has a reference set for `UnityEngine.dll`, but the path for this assembly is not set. To build the library, a reference path must be set to the Managed directory (Default is C:\Program Files\Unity\Editor\Data\Managed): 

## Installation
From a build or downloaded release, copy the `GradientGenerator.dll` to `[PROJECT DIR]\Assets\Plugins`

## Usage
There are four types of gradients that can be generated:

1. **Square Gradient**
   
   Code:
    
        using PixelsForGlory.GradientGenerator;
        ...
       
        var gradient = new SquareGradient(256, 256);
        float[,] results = gradient.Generate();
      
   Result:
      
      ![Square Gradient](../../../Screenshots/blob/master/\GradientGenerator/SquareGradient.png?raw=true "Square Gradient")

2. **Ramp Gradient**

   Code:
    
        using PixelsForGlory.GradientGenerator;
        ...
       
        var gradient = new RampGradient(256, 256);
        float[,] results = gradient.Generate();
       
       ![Ramp Gradient](../../../Screenshots/blob/master/\GradientGenerator/RampGradient.png?raw=true "Ramp Gradient")

3. **Spiral Gradient**
   
   There are a couple of options for creating a spiral gradient.  
   * extendToEdge: Instead of generating a circluar radient, this option extends to gradient all the way to the edge of the area.
   * divisions: The spiral and be divided into different divisions to create more interesting gradients.  There must be more than 2 divisions in the list and the points must be in ascending order (from 0 to 2 * PI).  The example below shows proper use.

   a. Spiral Gradient With No Divisions
  
   Code:
       
        using PixelsForGlory.GradientGenerator;
        ...
   
        var gradient = new SpiralGradient(0, 0, 128, 128, false);
        float[,] results = gradient.Generate();

   Result:
      
      ![Spiral Gradient](../../../Screenshots/blob/master/\GradientGenerator/SpiralGradientNoDivision.png?raw=true "Spiral Gradient")
      
   b. Spiral Gradient With Divisions
   
   Code:
       
        using PixelsForGlory.GradientGenerator;
        ...
   
        var gradient = new SpiralGradient(0, 0, 128, 128, true,
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
        float[,] results = gradient.Generate();

   Result:
      
      ![Spiral Gradient](../../../Screenshots/blob/master/\GradientGenerator/SpiralGradientDivision.png?raw=true "Spiral Gradient")
      
3. **Radial Gradient**
   
   There are a couple of options for creating a spiral gradient.  
   * extendToEdge: Instead of generating a circluar radient, this option extends to gradient all the way to the edge of the area.
   * divisions: The spiral and be divided into different divisions to create more interesting gradients.  There must be more than 2 divisions in the list and the points must be in ascending order (from 0 to 2 * PI).  The example below shows proper use.

   a. Radial Gradient With No Divisions
  
   Code:
       
        using PixelsForGlory.GradientGenerator;
        ...
   
        var gradient = new RadialGradient(0, 0, 128, 128, false);
        float[,] results = gradient.Generate();

   Result:
      
      ![Radial Gradient](../../../Screenshots/blob/master/\GradientGenerator/RadialGradientNoDivision.png?raw=true "Radial Gradient")
      
   b. Radial Gradient With Divisions
   
   Code:
       
        using PixelsForGlory.GradientGenerator;
        ...
   
        var gradient = new RadialGradient(0, 0, 128, 128, true,
           new List<RadialGradient.RadialGradientDivision>()
           {
              new RadialGradient.RadialGradientDivision { Value = 0f, Point = Vector2.zero },
              new RadialGradient.RadialGradientDivision { Value = 0f, Point = new Vector2(128.0f - (128.0f / 2 * 0.75f), 128.0f / 2 - (128.0f / 2 * 0.75f)) },
              new RadialGradient.RadialGradientDivision { Value = 0.9f, Point = new Vector2(128.0f - (128.0f * 0.25f), 128.0f - (128.0f * 0.25f)) },
              new RadialGradient.RadialGradientDivision { Value = 1f, Point = new Vector2(128.0f / 2, 128.0f / 2) }
          });
        float[,] results = gradient.Generate();

   Result:
      
      ![Radial Gradient](../../../Screenshots/blob/master/\GradientGenerator/RadialGradientDivision.png?raw=true "Radial Gradient")
   

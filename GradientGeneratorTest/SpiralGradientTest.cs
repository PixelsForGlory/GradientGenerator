// Copyright 2016 afuzzyllama. All Rights Reserved.
using System.Collections.Generic;
using System.Drawing;
using Color = System.Drawing.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelsForGlory.GradientGenerator;
using UnityEngine;

namespace GradientGeneratorTest
{
    [TestClass]
    public class SpiralGradientTest
    {
        [TestMethod]
        public void GenerateTest()
        {
            const int height = 256;
            const int width = 256;

            var gradient = new SpiralGradient(0, 0, width/2, height/2, false);
            float[,] results = gradient.Generate();

            using (var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap(@".\OriginalImages\SpiralGradientNoDivision.png"))
            {
                for (int x = 0; x < results.GetLength(0); x++)
                {
                    for (int y = 0; y < results.GetLength(1); y++)
                    {
                        int result = (byte)(255 * results[x, y]);
                        var color = Color.FromArgb(255, result, result, result);
                        testBitmap.SetPixel(x, y, color);
                    }
                }

                int differentPixels = 0;
                for (int x = 0; x < results.GetLength(0); x++)
                {
                    for (int y = 0; y < results.GetLength(1); y++)
                    {
                        if(testBitmap.GetPixel(x, y) != originalBitmap.GetPixel(x, y))
                        {
                            differentPixels++;
                        }
                    }
                }

                // Allow for a 0.5% difference
                Assert.IsTrue(differentPixels < Mathf.RoundToInt((width * height) * 0.005f));
            }

            gradient = new SpiralGradient(0, 0, width / 2, height / 2, true,
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
            results = gradient.Generate();
            using (var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap(@".\OriginalImages\SpiralGradientDivision.png"))
            {
                for (int x = 0; x < results.GetLength(0); x++)
                {
                    for (int y = 0; y < results.GetLength(1); y++)
                    {
                        int result = (byte)(255 * results[x, y]);
                        var color = Color.FromArgb(255, result, result, result);
                        testBitmap.SetPixel(x, y, color);
                    }
                }

                int differentPixels = 0;
                for (int x = 0; x < results.GetLength(0); x++)
                {
                    for (int y = 0; y < results.GetLength(1); y++)
                    {
                        if (testBitmap.GetPixel(x, y) != originalBitmap.GetPixel(x, y))
                        {
                            differentPixels++;
                        }
                    }
                }

                // Allow for a 0.5% difference
                Assert.IsTrue(differentPixels < Mathf.RoundToInt((width * height) * 0.005f));
            }
        }

        [TestMethod]
        public void GenerateWithBoundsTest()
        {
            const int height = 256;
            const int width = 256;
            const int divisionsX = 2;
            const int divisionsY = 2;
            const int divisionSizeX = width / divisionsX;
            const int divisionSizeY = height / divisionsY;

            var gradient = new SpiralGradient(0, 0, width / 2, height / 2, false);

            for (int divisionX = 0; divisionX < divisionsX; divisionX++)
            {
                for (int divisionY = 0; divisionY < divisionsY; divisionY++)
                {
                    float[,] results = gradient.Generate(divisionX * divisionSizeX, divisionY * divisionSizeY, divisionSizeX, divisionSizeY);

                    using (var testBitmap = new Bitmap(divisionSizeX, divisionSizeY))
                    using (var originalBitmap = new Bitmap(string.Format(@".\OriginalImages\SpiralGradient{0}_{1}.png", divisionX, divisionY)))
                    {
                        for (int x = 0; x < results.GetLength(0); x++)
                        {
                            for (int y = 0; y < results.GetLength(1); y++)
                            {
                                int result = (byte)(255 * results[x, y]);
                                var color = Color.FromArgb(255, result, result, result);
                                testBitmap.SetPixel(x, y, color);
                            }
                        }

                        int differentPixels = 0;
                        for (int x = 0; x < results.GetLength(0); x++)
                        {
                            for (int y = 0; y < results.GetLength(1); y++)
                            {
                                if (testBitmap.GetPixel(x, y) != originalBitmap.GetPixel(x, y))
                                {
                                    differentPixels++;
                                }
                            }
                        }

                        // Allow for a 0.5% difference
                        Assert.IsTrue(differentPixels < Mathf.RoundToInt((width * height) * 0.005f));
                    }
                }
            }
        }
    }
}

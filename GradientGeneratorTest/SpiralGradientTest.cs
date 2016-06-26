// Copyright 2016 afuzzyllama. All Rights Reserved.
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Color = System.Drawing.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelsForGlory.GradientGenerator;
using UnityEngine;

namespace GradientGeneratorTest
{
    [TestClass]
    [SuppressMessage("ReSharper", "RedundantExplicitArrayCreation")]
    public class SpiralGradientTest
    {
        [TestMethod]
        public void GenerateTest()
        {
            const int height = 256;
            const int width = 256;

            var iterationsX = new int[] { 64, 128, 192 };
            var iterationsY = new int[] { 64, 128, 192 };

            foreach (int iterationX in iterationsX)
            {
                foreach (int iterationY in iterationsY)
                {
                    var gradient = new SpiralGradient(iterationX, iterationY, width, height);
                    using (var testBitmap = new Bitmap(width, height))
                    using (
                        var originalBitmap =
                            new Bitmap(string.Format(@".\OriginalImages\SpiralGradient{0}_{1}.png", iterationX,
                                iterationY)))
                    {
                        for (int x = 0; x < width; x++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                int result = (byte)(255 * gradient.Generate(x, y));
                                var color = Color.FromArgb(255, result, result, result);
                                testBitmap.SetPixel(x, y, color);
                            }
                        }

                        int differentPixels = 0;
                        for (int x = 0; x < width; x++)
                        {
                            for (int y = 0; y < height; y++)
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

        [TestMethod]
        public void GenerateWithDivisionsTest()
        {
            const int height = 256;
            const int width = 256;

            var iterationsX = new int[] { 64, 128, 192 };
            var iterationsY = new int[] { 64, 128, 192 };

            foreach (int iterationX in iterationsX)
            {
                foreach (int iterationY in iterationsY)
                {
                    var gradient = new SpiralGradient(iterationX, iterationY, width, height,
                        new List<SpiralGradient.SpiralGradientDivision>()
                        {
                            new SpiralGradient.SpiralGradientDivision {Value = 0f, Point = 0f},
                            new SpiralGradient.SpiralGradientDivision {Value = 1f, Point = (2f * Mathf.PI) / 8},
                            new SpiralGradient.SpiralGradientDivision {Value = 0f, Point = 2 * (2f * Mathf.PI) / 8},
                            new SpiralGradient.SpiralGradientDivision {Value = 1f, Point = 3 * (2f * Mathf.PI) / 8},
                            new SpiralGradient.SpiralGradientDivision {Value = 0f, Point = 4 * (2f * Mathf.PI) / 8},
                            new SpiralGradient.SpiralGradientDivision {Value = 1f, Point = 5 * (2f * Mathf.PI) / 8},
                            new SpiralGradient.SpiralGradientDivision {Value = 0f, Point = 6 * (2f * Mathf.PI) / 8},
                            new SpiralGradient.SpiralGradientDivision {Value = 1f, Point = 7 * (2f * Mathf.PI) / 8},
                            new SpiralGradient.SpiralGradientDivision {Value = 0f, Point = 8 * (2f * Mathf.PI) / 8}
                        });
                    using (var testBitmap = new Bitmap(width, height))
                    using ( var originalBitmap = new Bitmap(string.Format(@".\OriginalImages\SpiralGradientWithDivisions{0}_{1}.png", iterationX,iterationY)))
                    {
                        for (int x = 0; x < width; x++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                int result = (byte)(255 * gradient.Generate(x, y));
                                var color = Color.FromArgb(255, result, result, result);
                                testBitmap.SetPixel(x, y, color);
                            }
                        }

                        int differentPixels = 0;
                        for (int x = 0; x < width; x++)
                        {
                            for (int y = 0; y < height; y++)
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

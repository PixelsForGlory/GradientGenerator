// Copyright 2016 afuzzyllama. All Rights Reserved.

using System;
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
    public class RadialGradientTest
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
                    var gradient = new RadialGradient(iterationX, iterationY, 1, 1, width, height, true);
                    using (var testBitmap = new Bitmap(width, height))
                    using (var originalBitmap = new Bitmap(String.Format(@".\OriginalImages\RadialGradient{0}_{1}.png", iterationX, iterationY)))
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
                    var gradient = new RadialGradient(iterationX, iterationY, 1, 1, width, height, true,
                        new List<RadialGradient.RadialGradientDivision>()
                        {
                            new RadialGradient.RadialGradientDivision { Value = 0f, Point = Vector2.zero },
                            new RadialGradient.RadialGradientDivision { Value = 0f, Point = new Vector2((float)width / 2 - ((float)width / 2 * 0.75f), (float)height / 2 - ((float)height / 2 * 0.75f)) },
                            new RadialGradient.RadialGradientDivision { Value = 0.9f, Point = new Vector2((float)width / 2 - ((float)width / 2 * 0.25f), (float)height / 2 - ((float)height / 2 * 0.25f)) },
                            new RadialGradient.RadialGradientDivision { Value = 1f, Point = new Vector2((float)width / 2, (float)height / 2) }
                        });

                    using (var testBitmap = new Bitmap(width, height))
                    using (var originalBitmap = new Bitmap(String.Format(@".\OriginalImages\RadialGradientWithDivisions{0}_{1}.png", iterationX, iterationY)))
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



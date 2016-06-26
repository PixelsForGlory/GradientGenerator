// Copyright 2016 afuzzyllama. All Rights Reserved.
using UnityEngine;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelsForGlory.GradientGenerator;
using Color = System.Drawing.Color;

namespace GradientGeneratorTest
{
    [TestClass]
    public class RampGradientTest
    {
        [TestMethod]
        public void GenerateTest()
        {
            const int height = 256;
            const int width = 256;

            var gradient = new RampGradient(width, height);
            using (var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap(@".\OriginalImages\RampGradient.png"))
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

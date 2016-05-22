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
        [DeploymentItem(@"OriginalImages\SpiralGradientNoDivision.png")]
        [DeploymentItem(@"OriginalImages\SpiralGradientDivision.png")]
        public void GenerateTest()
        {
            const int height = 256;
            const int width = 256;

            var gradient = new SpiralGradient(0, 0, width/2, height/2, false);
            float[,] results = gradient.Generate();

            using (var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap("./SpiralGradientNoDivision.png"))
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

                for (int x = 0; x < results.GetLength(0); x++)
                {
                    for (int y = 0; y < results.GetLength(1); y++)
                    {
                        Assert.AreEqual(testBitmap.GetPixel(x, y), originalBitmap.GetPixel(x, y));
                    }
                }
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
            using (var originalBitmap = new Bitmap("./SpiralGradientDivision.png"))
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

                for (int x = 0; x < results.GetLength(0); x++)
                {
                    for (int y = 0; y < results.GetLength(1); y++)
                    {
                        Assert.AreEqual(testBitmap.GetPixel(x, y), originalBitmap.GetPixel(x, y));
                    }
                }
            }
        }
    }
}

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
    public class RadialGradientTest
    {
        [TestMethod]
        [DeploymentItem(@"OriginalImages\RadialGradientNoDivision.png")]
        [DeploymentItem(@"OriginalImages\RadialGradientDivision.png")]
        public void GenerateTest()
        {
            const int height = 256;
            const int width = 256;

            var gradient = new RadialGradient(0, 0, width/2, height/2, false);
            float[,] results = gradient.Generate();

            using (var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap("./RadialGradientNoDivision.png"))
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

            gradient = new RadialGradient(0, 0, width / 2, height / 2, true,
                new List<RadialGradient.RadialGradientDivision>()
                {
                    new RadialGradient.RadialGradientDivision { Value = 0f, Point = Vector2.zero },
                    new RadialGradient.RadialGradientDivision { Value = 0f, Point = new Vector2((float)width / 2 - ((float)width / 2 * 0.75f), (float)height / 2 - ((float)height / 2 * 0.75f)) },
                    new RadialGradient.RadialGradientDivision { Value = 0.9f, Point = new Vector2((float)width / 2 - ((float)width / 2 * 0.25f), (float)height / 2 - ((float)height / 2 * 0.25f)) },
                    new RadialGradient.RadialGradientDivision { Value = 1f, Point = new Vector2((float)width / 2, (float)height / 2) }
                });
            results = gradient.Generate();
            using (var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap("./RadialGradientDivision.png"))
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

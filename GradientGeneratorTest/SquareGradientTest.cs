// Copyright 2016 afuzzyllama. All Rights Reserved.
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelsForGlory.GradientGenerator;

namespace GradientGeneratorTest
{
    [TestClass]
    public class SquareGradientTest
    {
        [TestMethod]
        [DeploymentItem(@"OriginalImages\SquareGradient.png")]
        public void GenerateTest()
        {
            const int height = 256;
            const int width = 256;

            var gradient = new SquareGradient(width, height);
            float[,] results = gradient.Generate();


            using(var testBitmap = new Bitmap(width, height))
            using(var originalBitmap = new Bitmap("./SquareGradient.png"))
            {
                for(int x = 0; x < results.GetLength(0); x++)
                {
                    for(int y = 0; y < results.GetLength(1); y++)
                    {
                        int result = (byte) (255 * results[x, y]);
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

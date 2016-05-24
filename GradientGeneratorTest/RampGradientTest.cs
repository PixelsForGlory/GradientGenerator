// Copyright 2016 afuzzyllama. All Rights Reserved.
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelsForGlory.GradientGenerator;

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
            float[,] results = gradient.Generate();

            using (var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap(@".\OriginalImages\RampGradient.png"))
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

        [TestMethod]
        public void GenerateWithBoundsTest()
        {
            const int height = 256;
            const int width = 256;
            const int divisionsX = 2;
            const int divisionsY = 2;
            const int divisionSizeX = width / divisionsX;
            const int divisionSizeY = height / divisionsY;

            var gradient = new RampGradient(width, height);

            for (int divisionX = 0; divisionX < divisionsX; divisionX++)
            {
                for (int divisionY = 0; divisionY < divisionsY; divisionY++)
                {
                    float[,] results = gradient.Generate(divisionX * divisionSizeX, divisionY * divisionSizeY, divisionSizeX, divisionSizeY);

                    using (var testBitmap = new Bitmap(divisionSizeX, divisionSizeY))
                    using (var originalBitmap = new Bitmap(string.Format(@".\OriginalImages\RampGradient{0}_{1}.png", divisionX, divisionY)))
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
    }
}

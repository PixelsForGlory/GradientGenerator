// Copyright 2016 afuzzyllama. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelsForGlory.GradientGenerator;
using Color = System.Drawing.Color;

namespace GradientGeneratorTest
{
    [TestClass]
    public class DiagonalRampGradientTest
    {
        [TestMethod]
        public void GenerateTest()
        {
            const int height = 256;
            const int width = 256;

            var gradient = new DiagonalRampGradient(width, height);
            using (var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap(@".\OriginalImages\DiagonalRampGradientXPYP.png"))
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

            gradient = new DiagonalRampGradient(width, height,
                new List<DiagonalRampGradient.DiagonalRampGradientDivision>()
                {
                    new DiagonalRampGradient.DiagonalRampGradientDivision()
                    {
                        Point = new Vector2(0, height),
                        Value = 0f
                    },
                    new DiagonalRampGradient.DiagonalRampGradientDivision()
                    {
                        Point = new Vector2(width, 0),
                        Value = 1f
                    }
                });
            using(var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap(@".\OriginalImages\DiagonalRampGradientXPYN.png"))
            {
                for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
                    {
                        int result = (byte)(255 * gradient.Generate(x, y));
                        var color = Color.FromArgb(255, result, result, result);
                        testBitmap.SetPixel(x, y, color);
                    }
                }

                int differentPixels = 0;
                for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
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

            gradient = new DiagonalRampGradient(width, height,
                new List<DiagonalRampGradient.DiagonalRampGradientDivision>()
                {
                    new DiagonalRampGradient.DiagonalRampGradientDivision()
                    {
                        Point = new Vector2(width, 0),
                        Value = 0f
                    },
                    new DiagonalRampGradient.DiagonalRampGradientDivision()
                    {
                        Point = new Vector2(0, height),
                        Value = 1f
                    }
                });
            using(var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap(@".\OriginalImages\DiagonalRampGradientXNYP.png"))
            {
                for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
                    {
                        int result = (byte)(255 * gradient.Generate(x, y));
                        var color = Color.FromArgb(255, result, result, result);
                        testBitmap.SetPixel(x, y, color);
                    }
                }

                int differentPixels = 0;
                for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
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

            gradient = new DiagonalRampGradient(width, height,
                new List<DiagonalRampGradient.DiagonalRampGradientDivision>()
                {
                    new DiagonalRampGradient.DiagonalRampGradientDivision()
                    {
                        Point = new Vector2(width, height),
                        Value = 0f
                    },
                    new DiagonalRampGradient.DiagonalRampGradientDivision()
                    {
                        Point = new Vector2(0, 0),
                        Value = 1f
                    }
                });
            using(var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap(@".\OriginalImages\DiagonalRampGradientXNYN.png"))
            {
                for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
                    {
                        int result = (byte)(255 * gradient.Generate(x, y));
                        var color = Color.FromArgb(255, result, result, result);
                        testBitmap.SetPixel(x, y, color);
                    }
                }

                int differentPixels = 0;
                for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
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

            gradient = new DiagonalRampGradient(width, height,
                new List<DiagonalRampGradient.DiagonalRampGradientDivision>()
                {
                    new DiagonalRampGradient.DiagonalRampGradientDivision()
                    {
                        Point = new Vector2(0, 0),
                        Value = 0f
                    },
                    new DiagonalRampGradient.DiagonalRampGradientDivision()
                    {
                        Point = new Vector2(width / 2f, height / 2f),
                        Value = 0f
                    },
                    new DiagonalRampGradient.DiagonalRampGradientDivision()
                    {
                        Point = new Vector2(width, height),
                        Value = 1f
                    }
                });
            using(var testBitmap = new Bitmap(width, height))
            using (var originalBitmap = new Bitmap(@".\OriginalImages\DiagonalRampGradientDivisions.png"))
            {
                for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
                    {
                        int result = (byte)(255 * gradient.Generate(x, y));
                        var color = Color.FromArgb(255, result, result, result);
                        testBitmap.SetPixel(x, y, color);
                    }
                }

                int differentPixels = 0;
                for(int x = 0; x < width; x++)
                {
                    for(int y = 0; y < height; y++)
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
        }
    }
}

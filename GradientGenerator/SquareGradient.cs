// Copyright 2016 afuzzyllama. All Rights Reserved.

namespace PixelsForGlory.GradientGenerator
{
    /// <summary>
    /// Generates a square gradient
    /// </summary>
    public class SquareGradient : Gradient
    {
        public SquareGradient(int lengthX, int lengthY) : base(lengthX, lengthY){}

        public override float[,] Generate()
        {
            for(int x = 0; x < LengthX; x++)
            {
                for(int y = 0; y < LengthY; y++)
                {
                    Values[x, y] = ((float) x / LengthXf + (float) y / LengthYf) / 2f;
                }
            }

            return Values;
        }
    }
}
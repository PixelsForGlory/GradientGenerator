// Copyright 2016 afuzzyllama. All Rights Reserved.

namespace PixelsForGlory.GradientGenerator
{
    /// <summary>
    /// Generates a ramp gradient
    /// </summary>
    public class RampGradient : Gradient
    {
        public RampGradient(int lengthX, int lengthY) : base(lengthX, lengthY){}

        public override float[,] Generate()
        {
            for(int x = 0; x < LengthX; x++)
            {
                for(int y = 0; y < LengthY; y++)
                {
                    Values[x, y] = (float) x / LengthXf;
                }
            }

            return Values;
        }
    }
}
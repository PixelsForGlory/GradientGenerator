// Copyright 2016 afuzzyllama. All Rights Reserved.

namespace PixelsForGlory.GradientGenerator
{
    /// <summary>
    /// Generates a ramp gradient
    /// </summary>
    public class RampGradient : Gradient
    {
        public RampGradient(int lengthX, int lengthY) : base(lengthX, lengthY){}

        public override float[,] Generate(int startX, int startY, int lengthX, int lengthY)
        {
            var values = new float[lengthX, lengthY];

            for (int x = 0; x < lengthX; x++)
            {
                for(int y = 0; y < lengthY; y++)
                {
                    values[x, y] = (startX + x) / LengthXf;
                }
            }

            return values;
        }
    }
}
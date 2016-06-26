// Copyright 2016 afuzzyllama. All Rights Reserved.

namespace PixelsForGlory.GradientGenerator
{
    /// <summary>
    /// Generates a ramp gradient
    /// </summary>
    public class RampGradient : Gradient
    {
        public RampGradient(int lengthX, int lengthY) : base(lengthX, lengthY){}

        public override float Generate(int x, int y)
        {
            return x / LengthXf;
        }
    }
}
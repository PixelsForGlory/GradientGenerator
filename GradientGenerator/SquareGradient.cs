// Copyright 2016 afuzzyllama. All Rights Reserved.

namespace PixelsForGlory.GradientGenerator
{
    /// <summary>
    /// Generates a square gradient
    /// </summary>
    public class SquareGradient : Gradient
    {
        public SquareGradient(int lengthX, int lengthY) : base(lengthX, lengthY){}

        public override float Generate(int x, int y)
        {
            return (x / LengthXf + y / LengthYf) / 2f;
        }
    }
}
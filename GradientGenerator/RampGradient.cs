// Copyright 2016 afuzzyllama. All Rights Reserved.
using System.Collections.Generic;

namespace PixelsForGlory.GradientGenerator
{
    /// <summary>
    /// Generates a ramp gradient
    /// </summary>
    public class RampGradient : Gradient
    {
        /// <summary>
        /// Represents a division of a ramp gradient 
        /// </summary>
        public struct RampGradientDivision
        {
            /// <summary>
            /// Point on an axis between 0 and LengthX or LengthY
            /// </summary>
            public float Point;

            /// <summary>
            /// The gradient value that the point represents
            /// </summary>
            public float Value;
        }

        private readonly IList<RampGradientDivision> _divisionsX;
        private readonly IList<RampGradientDivision> _divisionsY;

        public RampGradient(int lengthX, int lengthY, IList<RampGradientDivision> divisionsX = null, IList<RampGradientDivision> divisionsY = null)
            : base(lengthX, lengthY)
        {
            _divisionsX = divisionsX == null ? new List<RampGradientDivision>() : new List<RampGradientDivision>(divisionsX);
            _divisionsY = divisionsY == null ? new List<RampGradientDivision>() : new List<RampGradientDivision>(divisionsY);

            if(_divisionsX.Count == 0 && _divisionsY.Count == 0)
            {
                _divisionsX.Add(
                    new RampGradientDivision()
                    {
                        Point = 0f,
                        Value = 0f
                    });
                _divisionsX.Add(
                    new RampGradientDivision()
                    {
                        Point = LengthX,
                        Value = 1f
                    });
                _divisionsY.Add(
                    new RampGradientDivision()
                    {
                        Point = 0f,
                        Value = 0f
                    });
                _divisionsY.Add(
                    new RampGradientDivision()
                    {
                        Point = LengthY,
                        Value = 1f
                    });
            }
        }

        public override float Generate(int x, int y)
        {
            float numerator = 0f;
            float denominator = 0f;

            // Find the divisions the supplied point is between for both x and y.  
            // The two divisions will represent the min and max values to lerp between
            int currentDivisionIndex = 0;

            if(_divisionsX.Count != 0)
            {
                denominator += 1.0f;
                while((currentDivisionIndex + 1) < _divisionsX.Count)
                {
                    if (
                        x >= _divisionsX[currentDivisionIndex].Point
                        && x < _divisionsX[currentDivisionIndex + 1].Point)
                    {
                        break;
                    }
                    currentDivisionIndex++;
                }

                // If the current division + 1 is equal to the amount of divisions, 
                // assume the division is for the last segment
                if ((currentDivisionIndex + 1) == _divisionsX.Count)
                {
                    currentDivisionIndex--;
                }

                float t = (x - _divisionsX[currentDivisionIndex].Point) / (_divisionsX[currentDivisionIndex + 1].Point - _divisionsX[currentDivisionIndex].Point);

                numerator += Lerp(_divisionsX[currentDivisionIndex].Value, _divisionsX[currentDivisionIndex + 1].Value, t);
            }

            currentDivisionIndex = 0;
            if (_divisionsY.Count != 0)
            {
                denominator += 1.0f;
                while ((currentDivisionIndex + 1) < _divisionsY.Count)
                {
                    if (
                        y >= _divisionsY[currentDivisionIndex].Point
                        && y < _divisionsY[currentDivisionIndex + 1].Point)
                    {
                        break;
                    }
                    currentDivisionIndex++;
                }

                // If the current division + 1 is equal to the amount of divisions, 
                // assume the division is for the last segment
                if ((currentDivisionIndex + 1) == _divisionsY.Count)
                {
                    currentDivisionIndex--;
                }

                float t = (y - _divisionsY[currentDivisionIndex].Point) / (_divisionsY[currentDivisionIndex + 1].Point - _divisionsY[currentDivisionIndex].Point);

                numerator += Lerp(_divisionsY[currentDivisionIndex].Value, _divisionsY[currentDivisionIndex + 1].Value, t);
            }

            return numerator / denominator;
        }
    }
}
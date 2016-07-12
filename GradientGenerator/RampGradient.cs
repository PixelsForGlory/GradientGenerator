// Copyright 2016 afuzzyllama. All Rights Reserved.
using System;
using System.Collections.Generic;

namespace PixelsForGlory.GradientGenerator
{
    /// <summary>
    /// Generates a ramp gradient
    /// </summary>
    public class RampGradient : Gradient
    {
        /// <summary>
        /// Generation calculation type
        /// </summary>
        public enum GenerationCalculationType
        {
            Average,
            Min,
            Max
        }

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
        private readonly GenerationCalculationType _generationCalculationType;

        public RampGradient(int lengthX, int lengthY, IList<RampGradientDivision> divisionsX = null, IList<RampGradientDivision> divisionsY = null, GenerationCalculationType generationCalculationType = GenerationCalculationType.Average)
            : base(lengthX, lengthY)
        {
            _divisionsX = divisionsX == null ? new List<RampGradientDivision>() : new List<RampGradientDivision>(divisionsX);
            _divisionsY = divisionsY == null ? new List<RampGradientDivision>() : new List<RampGradientDivision>(divisionsY);
            _generationCalculationType = generationCalculationType;

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
            var values = new List<float>();

            // Find the divisions the supplied point is between for both x and y.  
            // The two divisions will represent the min and max values to lerp between
            int currentDivisionIndex = 0;

            if(_divisionsX.Count != 0)
            {
                while((currentDivisionIndex + 1) < _divisionsX.Count)
                {
                    if(
                        x >= _divisionsX[currentDivisionIndex].Point
                        && x < _divisionsX[currentDivisionIndex + 1].Point)
                    {
                        break;
                    }
                    currentDivisionIndex++;
                }

                // If the current division + 1 is equal to the amount of divisions, 
                // assume the division is for the last segment
                if((currentDivisionIndex + 1) == _divisionsX.Count)
                {
                    currentDivisionIndex--;
                }

                float t = (x - _divisionsX[currentDivisionIndex].Point) / (_divisionsX[currentDivisionIndex + 1].Point - _divisionsX[currentDivisionIndex].Point);

                values.Add(Lerp(_divisionsX[currentDivisionIndex].Value, _divisionsX[currentDivisionIndex + 1].Value, t));
            }

            currentDivisionIndex = 0;
            if(_divisionsY.Count != 0)
            {
                while((currentDivisionIndex + 1) < _divisionsY.Count)
                {
                    if(
                        y >= _divisionsY[currentDivisionIndex].Point
                        && y < _divisionsY[currentDivisionIndex + 1].Point)
                    {
                        break;
                    }
                    currentDivisionIndex++;
                }

                // If the current division + 1 is equal to the amount of divisions, 
                // assume the division is for the last segment
                if((currentDivisionIndex + 1) == _divisionsY.Count)
                {
                    currentDivisionIndex--;
                }

                float t = (y - _divisionsY[currentDivisionIndex].Point) / (_divisionsY[currentDivisionIndex + 1].Point - _divisionsY[currentDivisionIndex].Point);

                values.Add(Lerp(_divisionsY[currentDivisionIndex].Value, _divisionsY[currentDivisionIndex + 1].Value, t));
            }

            float returnValue;
            switch(_generationCalculationType)
            {
                case GenerationCalculationType.Average:
                    returnValue = 0f;
                    foreach(float number in values)
                    {
                        returnValue += number;
                    }
                    return returnValue / values.Count;
                case GenerationCalculationType.Min:
                    returnValue = float.MaxValue;
                    foreach(float number in values)
                    {
                        if(returnValue > number)
                        {
                            returnValue = number;
                        }
                    }
                    return returnValue;
                case GenerationCalculationType.Max:
                    returnValue = float.MinValue;
                    foreach(float number in values)
                    {
                        if(returnValue < number)
                        {
                            returnValue = number;
                        }
                    }
                    return returnValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
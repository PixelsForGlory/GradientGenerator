// Copyright 2016 afuzzyllama. All Rights Reserved.
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelsForGlory.GradientGenerator
{
    /// <summary>
    /// Generates a spiral gradient
    /// </summary>
    public class SpiralGradient : Gradient
    {
        /// <summary>
        /// Represents a division of a spiral gradient 
        /// </summary>
        public struct SpiralGradientDivision
        {
            /// <summary>
            /// Point on a circle (0 to 2pi) to divide on
            /// </summary>
            public float Point;

            /// <summary>
            /// The gradient value that the point represents
            /// </summary>
            public float Value;
        }

        /// <summary>
        /// Represents quadrant data for a spiral gradient
        /// </summary>
        protected class SpiralQuadrantData : QuadrantData
        {
            /// <summary>
            /// A list of divisions that the gradient will generate from
            /// </summary>
            private readonly List<SpiralGradientDivision> _divisions;

            /// <summary>
            /// Spiral quadrant data constructor
            /// </summary>
            /// <param name="minX">Left most point on a boundary</param>
            /// <param name="minY">Bottom most point on a bounary</param>
            /// <param name="lengthX">Length from minX to extend</param>
            /// <param name="lengthY">Length from minY to extend</param>
            /// <param name="quadrants">Quadrant or quadrants this instance represents</param>
            /// <param name="divisions"></param>
            public SpiralQuadrantData(
                int minX,
                int minY,
                int lengthX,
                int lengthY,
                CartesianQuadrant quadrants,
                IList<SpiralGradientDivision> divisions)
                : base(minX, minY, lengthX, lengthY, quadrants)
            {
                _divisions = new List<SpiralGradientDivision>(divisions);
            }

            public override float CalculateGradientValue(float angle, float x, float y)
            {
                // If the angle provided is not a number, assume it is the minimum value for the quadrant.
                // Otherwise, calculate the value of the angle for the quadrant this instance represents
                switch (Quadrants)
                {
                    case CartesianQuadrant.I:
                        if (float.IsNaN(angle))
                        {
                            angle = 0f;
                        }
                        else
                        {
                            angle = Mathf.PI / 2f - angle;
                        }
                        break;
                    case CartesianQuadrant.II:
                        if (float.IsNaN(angle))
                        {
                            angle = 3f * Mathf.PI;
                        }
                        else
                        {
                            angle = (3f * Mathf.PI) / 2f + angle;
                        }
                        break;
                    case CartesianQuadrant.III:
                        if (float.IsNaN(angle))
                        {
                            angle = Mathf.PI;
                        }
                        else
                        {
                            angle = (3f * Mathf.PI) / 2f - angle;
                        }
                        break;
                    case CartesianQuadrant.IV:
                        if (float.IsNaN(angle))
                        {
                            angle = Mathf.PI;
                        }
                        else
                        {
                            angle = Mathf.PI / 2f + angle;
                        }
                        break;
                    default:
                        throw new System.Exception(
                            "Cannot have a SpiralQuadrantData that holds more than an individual quadrant");
                }

                // Find the divisions the supplied angle is between.  
                // The two divisions will represent the min and max values to lerp between
                int currentDivisionIndex = 0;
                while (currentDivisionIndex + 1 < _divisions.Count)
                {
                    if (angle >= _divisions[currentDivisionIndex].Point &&
                       angle < _divisions[currentDivisionIndex + 1].Point)
                    {
                        break;
                    }
                    currentDivisionIndex++;
                }

                return Lerp(_divisions[currentDivisionIndex].Value, _divisions[currentDivisionIndex + 1].Value, (angle - _divisions[currentDivisionIndex].Point) / (_divisions[currentDivisionIndex + 1].Point - _divisions[currentDivisionIndex].Point));
            }
        }

        /// <summary>
        /// Center point of the gradient on the x axis
        /// </summary>
        private readonly int _centerPointX;

        /// <summary>
        /// Center point of the gradient on the y axis
        /// </summary>
        private readonly int _centerPointY;

        /// <summary>
        /// Cartesian quadrants for the gradient
        /// </summary>
        private readonly List<QuadrantData> _quadrantData;

        /// <summary>
        /// Spiral gradient constructor
        /// </summary>
        /// <param name="centerPointX">Center point of the gradient on the x-axis between 0 and LengthX</param>
        /// <param name="centerPointY">Center point of the gradient on the y-axis between 0 and LegnthY</param>
        /// <param name="lengthX">Length of gradient on the x-axis</param>
        /// <param name="lengthY">Length of gradient on the y-axis</param>
        /// <param name="divisions">Division of the gradient based on radius.  Ordered from 0,0 -> (LengthX/2, LengthY/2)</param>
        public SpiralGradient(
            int centerPointX, 
            int centerPointY, 
            int lengthX, 
            int lengthY, 
            List<SpiralGradientDivision> divisions = null)
            : base(lengthX, lengthY)
        {
            // If no divisions are supplied, generate basic divisions
            if (divisions == null)
            {
                divisions = new List<SpiralGradientDivision>
                {
                    new SpiralGradientDivision {Value = 0f, Point = 0f},
                    new SpiralGradientDivision {Value = 1f, Point = 2f * Mathf.PI}
                };
            }

            _centerPointX = centerPointX;
            _centerPointY = centerPointY;
            _quadrantData = new List<QuadrantData>()
            {
                new SpiralQuadrantData(_centerPointX, _centerPointY, LengthX - _centerPointX, LengthY - _centerPointY, CartesianQuadrant.I, divisions),
                new SpiralQuadrantData(0, _centerPointY, _centerPointX, LengthY - _centerPointY, CartesianQuadrant.II, divisions),
                new SpiralQuadrantData(0, 0, _centerPointX, _centerPointY, CartesianQuadrant.III, divisions),
                new SpiralQuadrantData(_centerPointX, 0, LengthX - _centerPointX, _centerPointY, CartesianQuadrant.IV, divisions)
            };
        }

        public override float Generate(int x, int y)
        {
            QuadrantData quadrantData = _quadrantData.First(item => item.Bounds.Contains(new Vector2(x, y)));

            // Find the angle between 0 degree point and current point
            var pointA = new Vector2(_centerPointX - x, _centerPointY - y);
            var pointC = new Vector2(0f, 0f);
            var pointB = new Vector2(_centerPointX - x, 0f);

            float a = Mathf.Sqrt(Mathf.Pow(pointB.x - pointC.x, 2f) + Mathf.Pow(pointB.y - pointC.y, 2f));
            float b = Mathf.Sqrt(Mathf.Pow(pointA.x - pointC.x, 2f) + Mathf.Pow(pointA.y - pointC.y, 2f));
            float c = Mathf.Sqrt(Mathf.Pow(pointA.x - pointB.x, 2f) + Mathf.Pow(pointA.y - pointB.y, 2f));

            float angle = Mathf.Acos((Mathf.Pow(a, 2f) + Mathf.Pow(b, 2f) - Mathf.Pow(c, 2f)) / (2f * a * b));

            return quadrantData.CalculateGradientValue(angle, LengthX, LengthY);
        }
    }
}
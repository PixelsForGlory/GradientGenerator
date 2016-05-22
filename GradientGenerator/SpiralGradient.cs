// Copyright 2016 afuzzyllama. All Rights Reserved.
using System.Collections.Generic;
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
            /// <param name="lengthX">Length of the quadrant on the x axis</param>
            /// <param name="lengthY">Length of the quadrant on the y axis</param>
            /// <param name="quadrants">What quadrants this instance represents</param>
            /// <param name="divisions">The division that will make up this gradient</param>
            public SpiralQuadrantData(int lengthX, int lengthY, CartesianQuadrant quadrants, List<SpiralGradientDivision> divisions) : base(lengthX, lengthY, quadrants)
            {
                _divisions = new List<SpiralGradientDivision>(divisions);
            }

            public override float CalculateGradientValue(float angle, float x, float y)
            {
                // If the angle provided is not a number, assume it is the minimum value for the quadrant.
                // Otherwise, calculate the value of the angle for the quadrant this instance represents
                switch(Quadrants)
                {
                    case CartesianQuadrant.I:
                        if(float.IsNaN(angle))
                        {
                            angle = 0f;
                        }
                        else
                        {
                            angle = Mathf.PI / 2f - angle;
                        }
                        break;
                    case CartesianQuadrant.II:
                        if(float.IsNaN(angle))
                        {
                            angle = 3f * Mathf.PI;
                        }
                        else
                        {
                            angle = (3f * Mathf.PI) / 2f + angle;
                        }
                        break;
                    case CartesianQuadrant.III:
                        if(float.IsNaN(angle))
                        {
                            angle = Mathf.PI;
                        }
                        else
                        {
                            angle = (3f * Mathf.PI) / 2f - angle;
                        }
                        break;
                    case CartesianQuadrant.IV:
                        if(float.IsNaN(angle))
                        {
                            angle = Mathf.PI / 2f;
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
                while(currentDivisionIndex + 1 < _divisions.Count)
                {
                    if(angle >= _divisions[currentDivisionIndex].Point &&
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
        /// Quadrant data for the gradient
        /// </summary>
        private readonly List<QuadrantData> _quadrantData;

        /// <summary>
        /// Spiral gradient constructor
        /// </summary>
        /// <param name="centerPointX">Center point of the gradient on the x axis</param>
        /// <param name="centerPointY">Center point of the gradient on the y axis</param>
        /// <param name="radiusX">Radius of the gradient on the x axis</param>
        /// <param name="radiusY">Radius of the gradient on the y axis</param>
        /// <param name="extendToEdge">Should the gradient generate a circle or push the values all the way to the edge of the generated square</param>
        /// <param name="divisions">Divisions that represent how this gradient should be generated</param>
        public SpiralGradient(int centerPointX, int centerPointY, int radiusX, int radiusY, bool extendToEdge, List<SpiralGradientDivision> divisions = null) 
            : base(radiusX * 2, radiusY * 2)
        {
            // If no divisions are supplied, generate basic divisions
            if(divisions == null)
            {
                divisions = new List<SpiralGradientDivision>
                {
                    new SpiralGradientDivision {Value = 0f, Point = 0f},
                    new SpiralGradientDivision {Value = 1f, Point = 2f * Mathf.PI}
                };
            }
            
            _centerPointX = centerPointX + radiusX;
            _centerPointY = centerPointY + radiusY;

            _quadrantData = new List<QuadrantData>();

            int radiusMultiplier = 1;
            if(extendToEdge)
            {
                radiusMultiplier = 2;
            }

            _quadrantData.Add(new SpiralQuadrantData(radiusX * radiusMultiplier, radiusY * radiusMultiplier, CartesianQuadrant.I, divisions));
            _quadrantData.Add(new SpiralQuadrantData(radiusX * radiusMultiplier, radiusY * radiusMultiplier, CartesianQuadrant.II, divisions));
            _quadrantData.Add(new SpiralQuadrantData(radiusX * radiusMultiplier, radiusY * radiusMultiplier, CartesianQuadrant.III, divisions));
            _quadrantData.Add(new SpiralQuadrantData(radiusX * radiusMultiplier, radiusY * radiusMultiplier, CartesianQuadrant.IV, divisions));
        }

        public override float[,] Generate()
        {
            // Initialize values
            for(int x = 0; x < LengthX; x++)
            {
                for(int y = 0; y < LengthY; y++)
                {
                    if(x == _centerPointX && y == _centerPointY)
                    {
                        Values[x, y] = 0f;
                    }
                    else
                    {
                        Values[x, y] = 1f;
                    }
                }
            }

            // Generate gradient values
            foreach(QuadrantData quadrantData in _quadrantData)
            {
                float startRadiusX, startRadiusY;
                float incrementRadiusX, incrementRadiusY;
                if(quadrantData.LengthX >= quadrantData.LengthY)
                {
                    startRadiusX = quadrantData.LengthXf / quadrantData.LengthYf;
                    incrementRadiusX = startRadiusX;
                    startRadiusY = 1f;
                    incrementRadiusY = 1f;
                }
                else //_radiusX < _radiusY
                {
                    startRadiusX = 1f;
                    incrementRadiusX = 1f;
                    startRadiusY = quadrantData.LengthYf / quadrantData.LengthXf;
                    incrementRadiusY = startRadiusY;
                }

                float currentRadiusX = startRadiusX;
                float currentRadiusY = startRadiusY;

                while(currentRadiusX < quadrantData.LengthXf && currentRadiusY < quadrantData.LengthYf)
                {
                    PlotEllipse(_centerPointX, _centerPointY, Mathf.RoundToInt(currentRadiusX), Mathf.RoundToInt(currentRadiusY), quadrantData, Values);

                    currentRadiusX += incrementRadiusX;
                    currentRadiusY += incrementRadiusY;
                }
            }

            return Values;
        }
    }
}
// Copyright 2016 afuzzyllama. All Rights Reserved.
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace PixelsForGlory.GradientGenerator
{
    /// <summary>
    /// Generates a radial gradient
    /// </summary>
    public class RadialGradient : Gradient
    {
        /// <summary>
        /// Represents a division of a radial gradient 
        /// </summary>
        public struct RadialGradientDivision
        {
            /// <summary>
            /// Radius of the ellipse to division on (0 to radiusX, 0 to radiusY)
            /// </summary>
            public Vector2 Point;

            /// <summary>
            /// The gradient value that the point represents
            /// </summary>
            public float Value;
        }

        /// <summary>
        /// Represents quadrant data for a radial gradient
        /// </summary>
        protected class RadialQuadrantData : QuadrantData
        {
            /// <summary>
            /// A list of divisions that the gradient will generate from
            /// </summary>
            private readonly List<RadialGradientDivision> _divisions;

            /// <summary>
            /// Radial quadrant data constructor
            /// </summary>
            /// <param name="lengthX">Length of the quadrant on the x axis</param>
            /// <param name="lengthY">Length of the quadrant on the y axis</param>
            /// <param name="quadrants">What quadrants this instance represents</param>
            /// <param name="divisions">The division that will make up this gradient</param>
            public RadialQuadrantData(int lengthX, int lengthY, CartesianQuadrant quadrants, List<RadialGradientDivision> divisions)
                : base(lengthX, lengthY, quadrants)
            {
                _divisions = divisions;
            }

            public override float CalculateGradientValue(float angle, float x, float y)
            {
                // Find the divisions the supplied point is between.  
                // The two divisions will represent the min and max values to lerp between
                int currentDivisionIndex = 0;
                while((currentDivisionIndex + 1) < _divisions.Count)
                {
                    if(
                        x >= _divisions[currentDivisionIndex].Point.x
                        && x < _divisions[currentDivisionIndex + 1].Point.x
                        && y >= _divisions[currentDivisionIndex].Point.y
                        && y < _divisions[currentDivisionIndex + 1].Point.y)
                    {
                        break;
                    }
                    currentDivisionIndex++;
                }

                // If the current division + 1 is equal to the amount of dividions, 
                // assume the division is for the last segment
                if((currentDivisionIndex + 1) == _divisions.Count)
                {
                    currentDivisionIndex--;
                }

                angle = float.IsNaN(angle) ? 0f : angle;

                float radiusContributionX = Mathf.Abs(angle % (Mathf.PI / 2));
                float radiusContributionY = 1f - radiusContributionX;

                // Calculate the radius over the range of the current division
                float numeratorRadiusX = x - _divisions[currentDivisionIndex].Point.x;
                float numeratorRadiusY = y - _divisions[currentDivisionIndex].Point.y;
                float denominatorRadiusX = _divisions[currentDivisionIndex + 1].Point.x - _divisions[currentDivisionIndex].Point.x;
                float denominatorRadiusY = _divisions[currentDivisionIndex + 1].Point.y - _divisions[currentDivisionIndex].Point.y;

                return
                    Mathf.Clamp(
                        Lerp(_divisions[currentDivisionIndex].Value, _divisions[currentDivisionIndex + 1].Value, numeratorRadiusX / denominatorRadiusX) * radiusContributionX + 
                        Lerp(_divisions[currentDivisionIndex].Value, _divisions[currentDivisionIndex + 1].Value, numeratorRadiusY / denominatorRadiusY) * radiusContributionY,
                        0f, 1f);
            }
        }

        private readonly int _centerPointX;
        private readonly int _centerPointY;
        private readonly List<QuadrantData> _quadrantData;
        private readonly List<RadialGradientDivision> _divisions;

        /// <summary>
        /// Radial Gradient constructor
        /// </summary>
        /// <param name="centerPointX">Center point of the gradient on the x axis</param>
        /// <param name="centerPointY">Center point of the gradient on the y axis</param>
        /// <param name="radiusX">Radius of the gradient on the x axis</param>
        /// <param name="radiusY">Radius of the gradient on the y axis</param>
        /// <param name="extendToEdge">Should the gradient extend all the way to the edges</param>
        /// <param name="divisions">Divisions that represent how this gradient should be generated.  Sorted from smallest point to largest point.</param>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public RadialGradient(
            int centerPointX,
            int centerPointY,
            int radiusX,
            int radiusY,
            bool extendToEdge,
            List<RadialGradientDivision> divisions = null) : base(radiusX * 2, radiusY * 2)
        {
            // If no divisions are supplied, generate basic divisions
            if(divisions == null)
            {
                _divisions = new List<RadialGradientDivision>
                {
                    new RadialGradientDivision() {Value = 0f, Point = Vector2.zero},
                    new RadialGradientDivision() {Value = 1f, Point = new Vector2(radiusX, radiusY)}
                };
            }
            else
            {
                _divisions = new List<RadialGradientDivision>(divisions);
            }

            _centerPointX = centerPointX + radiusX;
            _centerPointY = centerPointY + radiusY;

            _quadrantData = new List<QuadrantData>();

            // Generate quadrant data.  If not centered and clamped to bounds, create necessary quadrants to generate desired gradient
            if(centerPointX == 0 && centerPointY == 0 || extendToEdge == false)
            {
                _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY, CartesianQuadrant.All, _divisions));
            }
            else if(centerPointX == 0 && centerPointY != 0)
            {
                var clampedDivisionsY = new List<RadialGradientDivision>();
                foreach(RadialGradientDivision division in _divisions)
                {
                    clampedDivisionsY.Add(new RadialGradientDivision()
                    {
                        Value = division.Value,
                        Point =
                            new Vector2(division.Point.x,
                                division.Point.y * ((float) Mathf.Abs(radiusY - centerPointY) / radiusY))
                    });
                }

                if(centerPointY > 0)
                {
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY - centerPointY, CartesianQuadrant.I | CartesianQuadrant.II, clampedDivisionsY));
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY, CartesianQuadrant.III | CartesianQuadrant.IV, _divisions));
                }
                else // centerPointY < 0
                {
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY, CartesianQuadrant.I | CartesianQuadrant.II, _divisions));
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY + centerPointY, CartesianQuadrant.III | CartesianQuadrant.IV, clampedDivisionsY));
                }
            }
            else if(centerPointX != 0 && centerPointY == 0)
            {
                var clampedDivisionsX = new List<RadialGradientDivision>();
                foreach(RadialGradientDivision division in _divisions)
                {
                    clampedDivisionsX.Add(new RadialGradientDivision()
                    {
                        Value = division.Value,
                        Point =
                            new Vector2(
                                division.Point.x * ((float) Mathf.Abs(radiusX - centerPointX) / radiusX),
                                division.Point.y)
                    });
                }

                if(centerPointX > 0)
                {
                    _quadrantData.Add(new RadialQuadrantData(radiusX - centerPointX, radiusY, CartesianQuadrant.I | CartesianQuadrant.IV, clampedDivisionsX));
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY, CartesianQuadrant.II | CartesianQuadrant.III, _divisions));
                }
                else // centerPointX < 0
                {
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY,
                        CartesianQuadrant.I | CartesianQuadrant.IV, _divisions));
                    _quadrantData.Add(new RadialQuadrantData(radiusX + centerPointX, radiusY,
                        CartesianQuadrant.II | CartesianQuadrant.III, clampedDivisionsX));
                }
            }
            else // centerPointX != 0 && centerPointY != 0
            {
                var clampedDivisionsX = new List<RadialGradientDivision>();
                foreach(RadialGradientDivision division in _divisions)
                {
                    clampedDivisionsX.Add(new RadialGradientDivision()
                    {
                        Value = division.Value,
                        Point =
                            new Vector2(
                                division.Point.x * ((float) Mathf.Abs(radiusX - centerPointX) / radiusX),
                                division.Point.y)
                    });
                }

                var clampedDivisionsY = new List<RadialGradientDivision>();
                foreach(RadialGradientDivision division in _divisions)
                {
                    clampedDivisionsY.Add(new RadialGradientDivision()
                    {
                        Value = division.Value,
                        Point =
                            new Vector2(division.Point.x,
                                division.Point.y * ((float) Mathf.Abs(radiusY - centerPointY) / radiusY))
                    });
                }

                var clampedDivisionsXY = new List<RadialGradientDivision>();
                foreach(RadialGradientDivision division in _divisions)
                {
                    clampedDivisionsXY.Add(new RadialGradientDivision()
                    {
                        Value = division.Value,
                        Point =
                            new Vector2(
                                division.Point.x * ((float) Mathf.Abs(radiusX - centerPointX) / radiusX),
                                division.Point.y * ((float) Mathf.Abs(radiusY - centerPointY) / radiusY))
                    });
                }

                if(centerPointX > 0 && centerPointY > 0)
                {
                    _quadrantData.Add(new RadialQuadrantData(radiusX - centerPointX, radiusY - centerPointY, CartesianQuadrant.I, clampedDivisionsXY));
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY - centerPointY, CartesianQuadrant.II, clampedDivisionsY));
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY, CartesianQuadrant.III, _divisions));
                    _quadrantData.Add(new RadialQuadrantData(radiusX - centerPointX, radiusY, CartesianQuadrant.IV, clampedDivisionsX));
                }
                else if(centerPointX > 0 && centerPointY < 0)
                {
                    _quadrantData.Add(new RadialQuadrantData(radiusX - centerPointX, radiusY, CartesianQuadrant.I, clampedDivisionsX));
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY, CartesianQuadrant.II, _divisions));
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY + centerPointY, CartesianQuadrant.III, clampedDivisionsY));
                    _quadrantData.Add(new RadialQuadrantData(radiusX - centerPointX, radiusY + centerPointY, CartesianQuadrant.IV, clampedDivisionsXY));
                }
                else if(centerPointX < 0 && centerPointY > 0)
                {
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY - centerPointY, CartesianQuadrant.I, clampedDivisionsY));
                    _quadrantData.Add(new RadialQuadrantData(radiusX + centerPointX, radiusY - centerPointY, CartesianQuadrant.II, clampedDivisionsXY));
                    _quadrantData.Add(new RadialQuadrantData(radiusX + centerPointX, radiusY, CartesianQuadrant.III, clampedDivisionsX));
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY, CartesianQuadrant.IV, _divisions));
                }
                else //centerPointX < 0 && centerPointY < 0
                {
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY, CartesianQuadrant.I, _divisions));
                    _quadrantData.Add(new RadialQuadrantData(radiusX + centerPointX, radiusY, CartesianQuadrant.II, clampedDivisionsX));
                    _quadrantData.Add(new RadialQuadrantData(radiusX + centerPointX, radiusY + centerPointY, CartesianQuadrant.III, clampedDivisionsXY));
                    _quadrantData.Add(new RadialQuadrantData(radiusX, radiusY + centerPointY, CartesianQuadrant.IV, clampedDivisionsY));
                }
            }
        }

        public override float[,] Generate(int startX, int startY, int lengthX, int lengthY)
        {
            var values = new float[lengthX, lengthY];

            // Initialize values
            for(int x = 0; x < lengthX; x++)
            {
                for(int y = 0; y < lengthY; y++)
                {
                    if((startX + x) == _centerPointX && (startY + y) == _centerPointY)
                    {
                        values[x, y] = _divisions[0].Value;
                    }
                    else
                    {
                        values[x, y] = _divisions[_divisions.Count - 1].Value;
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
                    PlotEllipse(_centerPointX, _centerPointY, Mathf.RoundToInt(currentRadiusX), Mathf.RoundToInt(currentRadiusY), startX, startY, lengthX, lengthY, quadrantData, _divisions[_divisions.Count - 1].Value, values);

                    currentRadiusX += incrementRadiusX;
                    currentRadiusY += incrementRadiusY;
                }
            }

            return values;
        }
    }
}
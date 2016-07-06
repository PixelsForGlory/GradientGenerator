// Copyright 2016 afuzzyllama. All Rights Reserved.
using System.Collections.Generic;
using System.Linq;
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
            /// Radius of the ellipse to division on (0 to halfLengthX, 0 to halfLengthY)
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
            /// x in ratio of x:y
            /// </summary>
            public readonly float RatioX;

            /// <summary>
            /// y in ratio of x:y
            /// </summary>
            public readonly float RatioY;

            /// <summary>
            /// A list of divisions that the gradient will generate from
            /// </summary>
            private readonly List<RadialGradientDivision> _divisions;

            /// <summary>
            /// Radial quadrant data constructor
            /// </summary>
            /// <param name="minX">Left most point on a boundary</param>
            /// <param name="minY">Bottom most point on a bounary</param>
            /// <param name="lengthX">Length from minX to extend</param>
            /// <param name="lengthY">Length from minY to extend</param>
            /// <param name="ratioX">x in ratio of x:y</param>
            /// <param name="ratioY">y in ratio of x:y</param>
            /// <param name="quadrants">Quadrant or quadrants this instance represents</param>
            /// <param name="divisions"></param>
            public RadialQuadrantData(
                int minX, 
                int minY, 
                int lengthX, 
                int lengthY, 
                float ratioX, 
                float ratioY, 
                CartesianQuadrant quadrants, 
                IList<RadialGradientDivision> divisions)
                : base(minX, minY, lengthX, lengthY, quadrants)
            {
                RatioX = ratioX;
                RatioY = ratioY;
                _divisions = new List<RadialGradientDivision>(divisions);
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

                // If the current division + 1 is equal to the amount of divisions, 
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

        /// <summary>
        /// Center point of the gradient on the x-axis between 0 and LegnthX
        /// </summary>
        private readonly int _centerPointX;

        /// <summary>
        /// Center point of the gradient on the y-axis between 0 and LengthY
        /// </summary>
        private readonly int _centerPointY;

        /// <summary>
        /// Cartesian quadrants for the gradient
        /// </summary>
        private readonly List<QuadrantData> _quadrantData;

        /// <summary>
        /// Radial Gradient constructor
        /// </summary>
        /// <param name="centerPointX">Center point of the gradient on the x-axis between 0 and LengthX</param>
        /// <param name="centerPointY">Center point of the gradient on the y-axis between 0 and LegnthY</param>
        /// <param name="ratioX">x in ratio of x:y</param>
        /// <param name="ratioY">y in ratio of x:y</param>
        /// <param name="lengthX">Length of gradient on the x-axis</param>
        /// <param name="lengthY">Length of gradient on the y-axis</param>
        /// <param name="clampToEdge">Should this gradient clamp to the edges or extend beyond the edge</param>
        /// <param name="divisions">Division of the gradient based on radius.  Ordered from 0,0 -> (LengthX/2, LengthY/2)</param>
        public RadialGradient(
            int centerPointX,
            int centerPointY,
            int ratioX,
            int ratioY,
            int lengthX,
            int lengthY,
            bool clampToEdge,
            List<RadialGradientDivision> divisions = null) : base(lengthX, lengthY)
        {
            _centerPointX = centerPointX;
            _centerPointY = centerPointY;

            int halfLengthX = lengthX / 2;
            int halfLengthY = lengthY / 2;

            // If no divisions are supplied, generate basic divisions
            if( divisions == null)
            {
                divisions = new List<RadialGradientDivision>
                {
                    new RadialGradientDivision() { Value = 0f, Point = Vector2.zero }
                };

                divisions.Add(ratioX > ratioY
                    ? new RadialGradientDivision()
                    {
                        Value = 1f,
                        Point = new Vector2(halfLengthX, halfLengthX * ratioY / (float) ratioX)
                    }
                    : new RadialGradientDivision()
                    {
                        Value = 1f,
                        Point = new Vector2(halfLengthY * ratioX / (float) ratioY, halfLengthY)
                    });
            }
            else
            {
                divisions = new List<RadialGradientDivision>(divisions);
            }

            _quadrantData = new List<QuadrantData>();

            // Generate quadrant data.  If not centered and clamped to bounds, create necessary quadrants to generate desired gradient
            if((_centerPointX == halfLengthX && _centerPointY == halfLengthY) || clampToEdge == false)
            {
                _quadrantData.Add(new RadialQuadrantData(0, 0, LengthX, LengthY, ratioX, ratioY, CartesianQuadrant.I | CartesianQuadrant.II | CartesianQuadrant.III | CartesianQuadrant.IV, divisions));
            }
            else if(_centerPointX == halfLengthX && _centerPointY != halfLengthY)
            {
                if(_centerPointY > halfLengthY)
                {
                    var clampedDivisionsY = new List<RadialGradientDivision>();
                    float percentToUse = (LengthY - _centerPointY) / (float) halfLengthY;
                    foreach (RadialGradientDivision division in divisions)
                    {
                        clampedDivisionsY.Add(new RadialGradientDivision()
                        {
                            Value = division.Value,
                            Point = new Vector2(division.Point.x, division.Point.y * percentToUse)
                        });
                    }
                    _quadrantData.Add(new RadialQuadrantData(0, _centerPointY, LengthX, LengthY - _centerPointY, ratioX / percentToUse, ratioY, CartesianQuadrant.I | CartesianQuadrant.II, clampedDivisionsY));
                    _quadrantData.Add(new RadialQuadrantData(0, 0, LengthX, _centerPointY, ratioX, ratioY, CartesianQuadrant.III | CartesianQuadrant.IV, divisions));
                }
                else // _centerPointY <= halfLengthY
                {
                    var clampedDivisionsY = new List<RadialGradientDivision>();
                    float percentToUse = _centerPointY / (float)halfLengthY;
                    foreach (RadialGradientDivision division in divisions)
                    {
                        clampedDivisionsY.Add(new RadialGradientDivision()
                        {
                            Value = division.Value,
                            Point = new Vector2(division.Point.x, division.Point.y * percentToUse)
                        });
                    }
                    _quadrantData.Add(new RadialQuadrantData(0, _centerPointY, LengthX, LengthY - _centerPointY, ratioX, ratioY, CartesianQuadrant.I | CartesianQuadrant.II, divisions));
                    _quadrantData.Add(new RadialQuadrantData(0, 0, LengthX, _centerPointY, ratioX / percentToUse, ratioY, CartesianQuadrant.III | CartesianQuadrant.IV, clampedDivisionsY));
                }
            }
            else if(_centerPointX != halfLengthX && _centerPointY == halfLengthY)
            {
                if(_centerPointX > halfLengthX)
                {
                    var clampedDivisionsX = new List<RadialGradientDivision>();
                    float percentToUse = (LengthX - _centerPointX) / (float)halfLengthX;
                    foreach (RadialGradientDivision division in divisions)
                    {
                        clampedDivisionsX.Add(new RadialGradientDivision()
                        {
                            Value = division.Value,
                            Point = new Vector2(division.Point.x * percentToUse, division.Point.y)
                        });
                    }
                    _quadrantData.Add(new RadialQuadrantData(_centerPointX, 0, LengthX - _centerPointX, LengthY, ratioX, ratioY / percentToUse, CartesianQuadrant.I | CartesianQuadrant.IV, clampedDivisionsX));
                    _quadrantData.Add(new RadialQuadrantData(0, 0, _centerPointX, LengthY, ratioX, ratioY, CartesianQuadrant.II | CartesianQuadrant.III, divisions));
                }
                else // _centerPointX <= halfLengthX
                {
                    var clampedDivisionsX = new List<RadialGradientDivision>();
                    float percentToUse = _centerPointX / (float)halfLengthX;
                    foreach (RadialGradientDivision division in divisions)
                    {
                        clampedDivisionsX.Add(new RadialGradientDivision()
                        {
                            Value = division.Value,
                            Point = new Vector2(division.Point.x * percentToUse, division.Point.y)
                        });
                    }
                    _quadrantData.Add(new RadialQuadrantData(_centerPointX, 0, LengthX - _centerPointX, LengthY, ratioX, ratioY, CartesianQuadrant.I | CartesianQuadrant.IV, divisions));
                    _quadrantData.Add(new RadialQuadrantData(0, 0, _centerPointX, LengthY, ratioX, ratioY / percentToUse, CartesianQuadrant.II | CartesianQuadrant.III, clampedDivisionsX));
                }
            }
            else // _centerPointX != 0 && _centerPointY != 0
            {
                float percentToUseX;
                float percentToUseY;

                if (_centerPointX > halfLengthX)
                {
                    percentToUseX = (LengthX - _centerPointX) / (float) halfLengthX;
                }
                else
                {
                    percentToUseX = _centerPointX / (float)halfLengthX;
                }

                if (_centerPointY > halfLengthY)
                {
                    percentToUseY = (LengthY - _centerPointY) / (float)halfLengthY;
                }
                else
                {
                    percentToUseY = _centerPointY / (float)halfLengthY;
                }

                var clampedDivisionsX = new List<RadialGradientDivision>();
                foreach(RadialGradientDivision division in divisions)
                {
                    clampedDivisionsX.Add(new RadialGradientDivision()
                    {
                        Value = division.Value,
                        Point =
                            new Vector2(
                                division.Point.x * percentToUseX,
                                division.Point.y)
                    });
                }

                var clampedDivisionsY = new List<RadialGradientDivision>();
                foreach(RadialGradientDivision division in divisions)
                {
                    clampedDivisionsY.Add(new RadialGradientDivision()
                    {
                        Value = division.Value,
                        Point =
                            new Vector2(
                                division.Point.x,
                                division.Point.y * percentToUseY)
                    });
                }

                var clampedDivisionsXY = new List<RadialGradientDivision>();
                foreach(RadialGradientDivision division in divisions)
                {
                    clampedDivisionsXY.Add(new RadialGradientDivision()
                    {
                        Value = division.Value,
                        Point =
                            new Vector2(
                                division.Point.x * percentToUseX,
                                division.Point.y * percentToUseY)
                    });
                }

                if (_centerPointX > halfLengthX && _centerPointY > halfLengthY)
                {
                    _quadrantData.Add(new RadialQuadrantData(_centerPointX, _centerPointY, LengthX - _centerPointX, LengthY - _centerPointY, ratioX / percentToUseY, ratioY / percentToUseX, CartesianQuadrant.I, clampedDivisionsXY));
                    _quadrantData.Add(new RadialQuadrantData(0, _centerPointY, _centerPointX, LengthY - _centerPointY, ratioX / percentToUseY, ratioY, CartesianQuadrant.II, clampedDivisionsY));
                    _quadrantData.Add(new RadialQuadrantData(0, 0, _centerPointX, _centerPointY, ratioX, ratioY, CartesianQuadrant.III, divisions));
                    _quadrantData.Add(new RadialQuadrantData(_centerPointX, 0, LengthX - _centerPointX, _centerPointY, ratioX, ratioY / percentToUseX, CartesianQuadrant.IV, clampedDivisionsX));
                }
                else if (_centerPointX > halfLengthX && _centerPointY < halfLengthY)
                {
                    _quadrantData.Add(new RadialQuadrantData(_centerPointX, _centerPointY, LengthX - _centerPointX, LengthY - _centerPointY, ratioX, ratioY / percentToUseX, CartesianQuadrant.I, clampedDivisionsX));
                    _quadrantData.Add(new RadialQuadrantData(0, _centerPointY, _centerPointX, LengthY - _centerPointY, ratioX, ratioY, CartesianQuadrant.II, divisions));
                    _quadrantData.Add(new RadialQuadrantData(0, 0, _centerPointX, _centerPointY, ratioX / percentToUseY, ratioY, CartesianQuadrant.III, clampedDivisionsY));
                    _quadrantData.Add(new RadialQuadrantData(_centerPointX, 0, LengthX - _centerPointX, _centerPointY, ratioX / percentToUseY, ratioY / percentToUseX, CartesianQuadrant.IV, clampedDivisionsXY));
                }
                else if (_centerPointX < halfLengthX && _centerPointY > halfLengthY)
                {
                    _quadrantData.Add(new RadialQuadrantData(_centerPointX, _centerPointY, LengthX - _centerPointX, LengthY - _centerPointY, ratioX / percentToUseY, ratioY, CartesianQuadrant.I, clampedDivisionsY));
                    _quadrantData.Add(new RadialQuadrantData(0, _centerPointY, _centerPointX, LengthY - _centerPointY, ratioX / percentToUseY, ratioY / percentToUseX, CartesianQuadrant.II, clampedDivisionsXY));
                    _quadrantData.Add(new RadialQuadrantData(0, 0, _centerPointX, _centerPointY, ratioX, ratioY / percentToUseX, CartesianQuadrant.III, clampedDivisionsX));
                    _quadrantData.Add(new RadialQuadrantData(_centerPointX, 0, LengthX - _centerPointX, _centerPointY, ratioX, ratioY, CartesianQuadrant.IV, divisions));
                }
                else //_centerPointX < halfLengthX && _centerPointY < halfLengthY
                {
                    _quadrantData.Add(new RadialQuadrantData(_centerPointX, _centerPointY, LengthX - _centerPointX, LengthY - _centerPointY, ratioX, ratioY, CartesianQuadrant.I, divisions));
                    _quadrantData.Add(new RadialQuadrantData(0, _centerPointY, _centerPointX, LengthY - _centerPointY, ratioX, ratioY / percentToUseX, CartesianQuadrant.II, clampedDivisionsX));
                    _quadrantData.Add(new RadialQuadrantData(0, 0, _centerPointX, _centerPointY, ratioX / percentToUseY, ratioY / percentToUseX, CartesianQuadrant.III, clampedDivisionsXY));
                    _quadrantData.Add(new RadialQuadrantData(_centerPointX, 0, LengthX - _centerPointX, _centerPointY, ratioX / percentToUseY, ratioY, CartesianQuadrant.IV, clampedDivisionsY));
                }
            }
        }

        public override float Generate(int x, int y)
        {
            RadialQuadrantData quadrantData =  (RadialQuadrantData)_quadrantData.First(item => item.Bounds.Contains(new Vector2(x, y)));

            // Find the angle between 0 degree point and current point
            var pointA = new Vector2(_centerPointX - x, _centerPointY - y);
            var pointC = new Vector2(0f, 0f);
            var pointB = new Vector2(_centerPointX - x, 0f);

            float a = Mathf.Sqrt(Mathf.Pow(pointB.x - pointC.x, 2f) + Mathf.Pow(pointB.y - pointC.y, 2f));
            float b = Mathf.Sqrt(Mathf.Pow(pointA.x - pointC.x, 2f) + Mathf.Pow(pointA.y - pointC.y, 2f));
            float c = Mathf.Sqrt(Mathf.Pow(pointA.x - pointB.x, 2f) + Mathf.Pow(pointA.y - pointB.y, 2f));

            float angle = Mathf.Acos((Mathf.Pow(a, 2f) + Mathf.Pow(b, 2f) - Mathf.Pow(c, 2f)) / (2f * a * b));

            // Calculate radius to get the x axis length and the y-axis length
            float radius = Mathf.Sqrt(Mathf.Pow(_centerPointX - x, 2) / Mathf.Pow(quadrantData.RatioX, 2) + Mathf.Pow(_centerPointY - y, 2) / Mathf.Pow(quadrantData.RatioY, 2));

            // equation for an ellipse = x^2 / (ratioX^2 * r^2) + y^2 / (ratioY^2 * r^2) = 1
            // x-axis length = Sqrt(ratioX^2 * r^2)
            // y-axis length = Sqrt(ratioY^2 * r^2)
            float currentLengthX = Mathf.Sqrt(Mathf.Pow(quadrantData.RatioX, 2) * Mathf.Pow(radius, 2));
            float currentLengthY = Mathf.Sqrt(Mathf.Pow(quadrantData.RatioY, 2) * Mathf.Pow(radius, 2));

            return quadrantData.CalculateGradientValue(angle, currentLengthX, currentLengthY);
        }
    }
}
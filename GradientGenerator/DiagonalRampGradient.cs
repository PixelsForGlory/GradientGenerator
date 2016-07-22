// Copyright 2016 afuzzyllama. All Rights Reserved.
using System.Collections.Generic;
using UnityEngine;

namespace PixelsForGlory.GradientGenerator
{
    /// <summary>
    /// Generates a diagonal ramp gradient
    /// </summary>
    public class DiagonalRampGradient : Gradient
    {
        /// <summary>
        /// Represents a division of a ramp gradient 
        /// </summary>
        public struct DiagonalRampGradientDivision
        {
            /// <summary>
            /// Point on an axis between 0,0 and LengthX, LengthY
            /// </summary>
            public Vector2 Point;

            /// <summary>
            /// The gradient value that the point represents
            /// </summary>
            public float Value;
        }

        private readonly IList<DiagonalRampGradientDivision> _divisionsXY;

        public DiagonalRampGradient(int lengthX, int lengthY, IList<DiagonalRampGradientDivision> divisionsXY = null)
            : base(lengthX, lengthY)
        {
            _divisionsXY = divisionsXY == null ? new List<DiagonalRampGradientDivision>() : new List<DiagonalRampGradientDivision>(divisionsXY);

            if(_divisionsXY.Count == 0)
            {
                _divisionsXY.Add(
                    new DiagonalRampGradientDivision()
                    {
                        Point = Vector2.zero,
                        Value = 0f
                    });
                _divisionsXY.Add(
                    new DiagonalRampGradientDivision()
                    {
                        Point = new Vector2(LengthXf, LengthYf),
                        Value = 1f
                    });
            }
        }

        public override float Generate(int x, int y)
        {
            // Find the 2 divisions that creates a line that has a perpendicular intersection with the passed in point that lines between the
            // two points that created the line
            int currentDivisionIndex = 0;

            while((currentDivisionIndex + 1) < _divisionsXY.Count)
            {
                // y = mx + b
                // Get division slope and x intersect
                float divisionMX = _divisionsXY[currentDivisionIndex + 1].Point.x - _divisionsXY[currentDivisionIndex].Point.x;
                float divisionMY = _divisionsXY[currentDivisionIndex + 1].Point.y - _divisionsXY[currentDivisionIndex].Point.y;
                float divisionM = divisionMY / divisionMX;
                float divisionB = _divisionsXY[currentDivisionIndex].Point.y - divisionM * _divisionsXY[currentDivisionIndex].Point.x;

                // Get point slope
                // perpendicular slope is negative inverse of target line
                float pointM = -1 / divisionM;
                float pointB = y - pointM * x;

                // Intersection solve
                // x = (pointB - divisionB) / (divisionM - pointM)
                float intersectX = (pointB - divisionB) / (divisionM - pointM);
                float intersectY = divisionM * intersectX + divisionB;

                if(
                    (
                        divisionMX > 0f && divisionMY > 0f
                        &&(
                            intersectX >= _divisionsXY[currentDivisionIndex].Point.x
                            && intersectX < _divisionsXY[currentDivisionIndex + 1].Point.x
                            && intersectY >= _divisionsXY[currentDivisionIndex].Point.y
                            && intersectY < _divisionsXY[currentDivisionIndex + 1].Point.y
                        )
                    )
                    || (
                        divisionMX > 0f && divisionMY < 0f
                        && (
                            intersectX >= _divisionsXY[currentDivisionIndex].Point.x
                            && intersectX < _divisionsXY[currentDivisionIndex + 1].Point.x
                            && intersectY < _divisionsXY[currentDivisionIndex].Point.y
                            && intersectY >=_divisionsXY[currentDivisionIndex + 1].Point.y
                        )
                    )||(
                        divisionMX < 0f && divisionMY > 0f
                        && (
                            intersectX < _divisionsXY[currentDivisionIndex].Point.x
                            && intersectX >= _divisionsXY[currentDivisionIndex + 1].Point.x
                            && intersectY >= _divisionsXY[currentDivisionIndex].Point.y
                            && intersectY < _divisionsXY[currentDivisionIndex + 1].Point.y
                        )
                    ) || (
                        divisionMX < 0f && divisionMY < 0f
                        && (
                            intersectX < _divisionsXY[currentDivisionIndex].Point.x
                            && intersectX >= _divisionsXY[currentDivisionIndex + 1].Point.x
                            && intersectY < _divisionsXY[currentDivisionIndex].Point.y
                            && intersectY >= _divisionsXY[currentDivisionIndex + 1].Point.y
                        )
                    )
                )
                { 
                    // find percent that intersection is of total line segment
                    float divisionLength = Mathf.Sqrt(Mathf.Pow(_divisionsXY[currentDivisionIndex + 1].Point.x - _divisionsXY[currentDivisionIndex].Point.x, 2) + Mathf.Pow(_divisionsXY[currentDivisionIndex + 1].Point.y - _divisionsXY[currentDivisionIndex].Point.y, 2));
                    float intersectionLegnth = Mathf.Sqrt(Mathf.Pow(intersectX - _divisionsXY[currentDivisionIndex].Point.x, 2) + Mathf.Pow(intersectY - _divisionsXY[currentDivisionIndex].Point.y, 2));
                    float t = intersectionLegnth / divisionLength;
                    return  Lerp(_divisionsXY[currentDivisionIndex].Value, _divisionsXY[currentDivisionIndex + 1].Value, t);
                }

                currentDivisionIndex++;
            }
            return 0f;
        }
    }
}
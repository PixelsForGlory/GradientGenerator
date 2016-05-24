// Copyright 2016 afuzzyllama. All Rights Reserved.
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace PixelsForGlory.GradientGenerator
{
    /// <summary>
    /// Abstract gradient class as a starting point for generating gradients
    /// </summary>
    public abstract class Gradient
    {
        /// <summary>
        /// Length of gradient on the x axis
        /// </summary>
        public readonly int LengthX;

        /// <summary>
        /// Length of the gradient on the y axis
        /// </summary>
        public readonly int LengthY;

        /// <summary>
        /// Float representation of the x length
        /// </summary>
        public readonly float LengthXf;

        /// <summary>
        /// Float representation of the y length
        /// </summary>
        public readonly float LengthYf;

        /// <summary>
        /// Gradient constructor
        /// </summary>
        /// <param name="lengthX">Length of gradient on the x axis</param>
        /// <param name="lengthY">Length of gradient on the y axis</param>
        protected Gradient(int lengthX, int lengthY)
        {
            LengthX = lengthX;
            LengthY = lengthY;
            LengthXf = LengthX;
            LengthYf = LengthY;
        }

        /// <summary>
        /// Generate the full gradient
        /// </summary>
        /// <returns>2D array of gradient values</returns>
        public float[,] Generate()
        {
            return Generate(0, LengthX - 1, 0, LengthY - 1);
        }

        /// <summary>
        /// Generate part of the gradient
        /// </summary>
        /// <param name="startX">Start point to generate on the x axis</param>
        /// <param name="startY">Start point to generate on the y axis</param>
        /// <param name="lengthX">Length out from the start point to go on the x axis</param>
        /// <param name="lengthY">Length out from the start point to go on the y axis</param>
        /// <returns></returns>
        public abstract float[,] Generate(int startX, int startY, int lengthX, int lengthY);

        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        protected enum CartesianQuadrant : byte
        {
            None = 0x0,
            I = 0x01,
            II = 0x02,
            III = 0x04,
            IV = 0x08,
            All = 0x10
        }

        /// <summary>
        /// Protected interface that contains all methods needed to complete a point on the gradient calculation
        /// </summary>
        protected interface IGradientCalculation
        {
            /// <summary>
            /// Calculates the value of the gradient at the current angle and radius
            /// </summary>
            /// <param name="angle">Number of radians from 0 for the current point</param>
            /// <param name="x">x to calculate value for</param>
            /// <param name="y">y to calculate value for</param>
            /// <returns></returns>
            float CalculateGradientValue(
                float angle,
                float x,
                float y);
        }

        /// <summary>
        /// Abstract quadrant data class that can represent one or more cartesian quandrants
        /// </summary>
        protected abstract class QuadrantData : IGradientCalculation
        {
            public readonly int LengthX;
            public readonly int LengthY;
            public readonly float LengthXf;
            public readonly float LengthYf;
            public readonly CartesianQuadrant Quadrants;

            /// <summary>
            /// Quadrant data constructor
            /// </summary>
            /// <param name="lengthX">Length of the quadrant on the x axis</param>
            /// <param name="lengthY">Length of the quadrant on the y axis</param>
            /// <param name="quadrants">What quadrants will be represented by this instance</param>
            protected QuadrantData(int lengthX, int lengthY, CartesianQuadrant quadrants)
            {
                LengthX = lengthX;
                LengthXf = lengthX;

                LengthY = lengthY;
                LengthYf = lengthY;

                Quadrants = quadrants;
            }

            public abstract float CalculateGradientValue(
                float angle,
                float x,
                float y);
        }

        /// <summary>
        /// Plot a filled in ellipse
        /// </summary>
        /// <param name="centerX">X value for the center of the ellipse</param>
        /// <param name="centerY">Y value for the center of the ellipse</param>
        /// <param name="radiusX">Radius of the ellipse on the x axis</param>
        /// <param name="radiusY">Radius of the ellipse on the y axis</param>
        /// <param name="startX">Start point to generate on the x axis</param>
        /// <param name="startY">Start point to generate on the y axis</param>
        /// <param name="lengthX">Length out from the start point to go on the x axis</param>
        /// <param name="lengthY">Length out from the start point to go on the y axis</param>
        /// <param name="quadrantData">What quadrants are being plotted</param>
        /// <param name="result">The resulting gradient values from the method</param>
        protected void PlotEllipse(int centerX, int centerY, int radiusX, int radiusY, int startX, int startY, int lengthX, int lengthY, QuadrantData quadrantData, float[,] result)
        {
            // If the radius on either axis is 0 there is nothing to do here
            if(radiusX == 0 || radiusY == 0)
            {
                Debug.LogWarning("Radius of 0 passed to PlotEllipse");
                return;
            }

            int radius2XSquared = 2 * radiusX * radiusX;
            int radius2YSquared = 2 * radiusY * radiusY;

            int x = radiusX;
            int y = 0;

            int changeX = radiusY * radiusY * (1 - 2 * radiusX);
            int changeY = radiusX * radiusX;

            int error = 0;

            int stoppingX = radius2YSquared * radiusX;
            int stoppingY = 0;

            // 1st set of points y' > -1
            while(stoppingX >= stoppingY)
            {
                PlotAndFillEllipseArea(centerX, centerY, x, y, radiusX, radiusY, startX, startY, lengthX, lengthY, quadrantData, result);

                y++;
                stoppingY += radius2XSquared;
                error += changeY;
                changeY += radius2XSquared;

                if(2 * error + changeX > 0)
                {
                    x--;
                    stoppingX -= radius2YSquared;
                    error += changeX;
                    changeX += radius2YSquared;
                }
            }

            // 1st point set is done, start the 2nd set
            x = 0;
            y = radiusY;
            changeX = radiusY * radiusY;
            changeY = radiusX * radiusX * (1 - 2 * radiusY);
            error = 0;
            stoppingX = 0;
            stoppingY = radius2XSquared * radiusY;

            // 2nd set of points y' < -1
            while(stoppingX <= stoppingY)
            {
                PlotAndFillEllipseArea(centerX, centerY, x, y, radiusX, radiusY, startX, startY, lengthX, lengthY, quadrantData, result);
                x++;
                stoppingX += radius2YSquared;
                error += changeX;
                changeX += radius2YSquared;
                if(2 * error + changeY > 0)
                {
                    y--;
                    stoppingY -= radius2XSquared;
                    error += changeY;
                    changeY += radius2XSquared;
                }
            }
        }

        /// <summary>
        /// Represents a quadrilateral are to plot gradient values in
        /// </summary>
        private struct PlotData
        {
            public readonly int StartX;
            public readonly int EndX;

            public readonly int StartY;
            public readonly int EndY;

            public PlotData(int startX, int endX, int startY, int endY) : this()
            {
                StartX = startX;
                EndX = endX;
                StartY = startY;
                EndY = endY;
            }
        }

        /// <summary>
        /// Plots a filled in area of an ellipse
        /// </summary>
        /// <param name="centerX">Origin of the ellipse on the x axis</param>
        /// <param name="centerY">Origin of the ellipse on the y axis</param>
        /// <param name="x">Current x value to calculate for</param>
        /// <param name="y">Current y value to calculate for</param>
        /// <param name="currentRadiusX">Current radius on the x axis to calculate for</param>
        /// <param name="currentRadiusY">Current radius on the y axis to calculate for</param>
        /// <param name="startX">Start point to generate on the x axis</param>
        /// <param name="startY">Start point to generate on the y axis</param>
        /// <param name="lengthX">Length out from the start point to go on the x axis</param>
        /// <param name="lengthY">Length out from the start point to go on the y axis</param>
        /// <param name="quadrantData">What quadrants are being calculated for</param>
        /// <param name="result">The gradient results from the method</param>
        private void PlotAndFillEllipseArea(int centerX, int centerY, int x, int y, float currentRadiusX, float currentRadiusY, int startX, int startY, int lengthX, int lengthY, QuadrantData quadrantData, float[,] result)
        {
            // Find the angle between 0 degree point and current point
            var pointC = Vector2.zero;
            var pointA = new Vector2(x, y);
            var pointB = new Vector2(x, 0f);

            float a = Mathf.Sqrt(Mathf.Pow(pointB.x - pointC.x, 2f) + Mathf.Pow(pointB.y - pointC.y, 2f));
            float b = Mathf.Sqrt(Mathf.Pow(pointA.x - pointC.x, 2f) + Mathf.Pow(pointA.y - pointC.y, 2f));
            float c = Mathf.Sqrt(Mathf.Pow(pointA.x - pointB.x, 2f) + Mathf.Pow(pointA.y - pointB.y, 2f));

            float angle = Mathf.Acos((Mathf.Pow(a, 2f) + Mathf.Pow(b, 2f) - Mathf.Pow(c, 2)) / (2 * a * b));

            // Calculate the area that will be processed
            PlotData originalPlotData;
            if ((quadrantData.Quadrants & CartesianQuadrant.All) == CartesianQuadrant.All)
            {
                originalPlotData = new PlotData(centerX - x, centerX + x, centerY - y, centerY + y);
            }
            else if((quadrantData.Quadrants & (CartesianQuadrant.I | CartesianQuadrant.II)) == (CartesianQuadrant.I | CartesianQuadrant.II))
            {
                originalPlotData = new PlotData(centerX - x, centerX + x, centerY, centerY + y);
            }
            else if((quadrantData.Quadrants & (CartesianQuadrant.II | CartesianQuadrant.III)) == (CartesianQuadrant.II | CartesianQuadrant.III))
            {
                originalPlotData = new PlotData(centerX - x, centerX, centerY - y, centerY + y);
            }
            else if((quadrantData.Quadrants & (CartesianQuadrant.III | CartesianQuadrant.IV)) == (CartesianQuadrant.III | CartesianQuadrant.IV))
            {
                originalPlotData = new PlotData(centerX - x, centerX + x, centerY - y, centerY);
            }
            else if((quadrantData.Quadrants & (CartesianQuadrant.I | CartesianQuadrant.IV)) == (CartesianQuadrant.I | CartesianQuadrant.IV))
            {
                originalPlotData = new PlotData(centerX, centerX + x, centerY - y, centerY + y);
            }
            else if((quadrantData.Quadrants & CartesianQuadrant.I) == CartesianQuadrant.I)
            {
                originalPlotData = new PlotData(centerX, centerX + x, centerY, centerY + y);
            }
            else if((quadrantData.Quadrants & CartesianQuadrant.II) == CartesianQuadrant.II)
            {
                originalPlotData = new PlotData(centerX - x, centerX, centerY, centerY + y);
            }
            else if((quadrantData.Quadrants & CartesianQuadrant.III) == CartesianQuadrant.III)
            {
                originalPlotData = new PlotData(centerX - x, centerX, centerY - y, centerY);
            }
            else // ((quadrantData & CartesianQuadrant.IV) == CartesianQuadrant.IV)
            {
                originalPlotData = new PlotData(centerX, centerX + x, centerY - y, centerY);
            }

            var bounds = new PlotData(startX, startX + lengthX - 1, startY, startY + lengthY - 1);
            var plotData = new PlotData(
                Mathf.Max(bounds.StartX, originalPlotData.StartX),
                Mathf.Min(bounds.EndX, originalPlotData.EndX),
                Mathf.Max(bounds.StartY, originalPlotData.StartY),
                Mathf.Min(bounds.EndY, originalPlotData.EndY));

            // Generate result values
            for (int currentX = plotData.StartX; currentX <= plotData.EndX; currentX++)
            {
                for(int currentY = plotData.StartY; currentY <= plotData.EndY; currentY++)
                {
                    if (
                    currentX < LengthX &&
                    currentX >= 0 &&
                    currentY < LengthY &&
                    currentY >= 0 &&
                    Math.Abs(result[currentX - startX, currentY - startY] - 1f) < 0.000001f
                    )
                    {
                        result[currentX - startX, currentY - startY] = quadrantData.CalculateGradientValue(angle, currentRadiusX, currentRadiusY);
                    }
                }
            }
        }

        /// <summary>
        /// Lerp between two points
        /// </summary>
        /// <param name="pointStart">Starting point</param>
        /// <param name="pointEnd">Ending point</param>
        /// <param name="t">t value between 0 to 1</param>
        /// <returns>Lerped value</returns>
        protected static float Lerp(float pointStart, float pointEnd, float t)
        {
            return (1 - t) * pointStart + t * pointEnd;
        }
    }
}
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

        ///// <summary>
        ///// Generate the full gradient
        ///// </summary>
        ///// <returns>2D array of gradient values</returns>
        //public float[,] Generate()
        //{
        //    return Generate(0, 0, LengthX, LengthY);
        //}

        /// <summary>
        /// Generate part of the gradient
        /// </summary>
        /// <param name="x">x value from 0 to LengthX</param>
        /// <param name="y">y value from 0 to LengthY</param>
        /// <returns></returns>
        public abstract float Generate(int x, int y);

        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        protected enum CartesianQuadrant : byte
        {
            I = 0x01,
            II = 0x02,
            III = 0x04,
            IV = 0x08,
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
            public Rect Bounds {get; private set; }
            public readonly CartesianQuadrant Quadrants;

            /// <summary>
            /// Quadrant data constructor
            /// </summary>
            /// <param name="minX">Left most point on a boundary</param>
            /// <param name="minY">Bottom most point on a bounary</param>
            /// <param name="lengthX">Length from minX to extend</param>
            /// <param name="lengthY">Length from minY to extend</param>
            /// <param name="quadrants">What quadrants will be represented by this instance</param>
            protected QuadrantData(int minX, int minY, int lengthX, int lengthY, CartesianQuadrant quadrants)
            {
                Bounds = new Rect(minX, minY, lengthX, lengthY);
                Quadrants = quadrants;
            }

            public abstract float CalculateGradientValue(
                float angle,
                float x,
                float y);
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
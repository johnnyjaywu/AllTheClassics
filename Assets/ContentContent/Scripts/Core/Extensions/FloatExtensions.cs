﻿namespace ContentContent.Core
{
    public static class FloatExtensions
    {
        /// <summary>Determines if the numbers are equal with a certain tolerance.</summary>
        /// <param name="firstNum">First number.</param>
        /// <param name="secondNum">Second number.</param>
        /// <param name="tolerance">Tolerance by which to determine if the numbers are equal. Default is 0.01f.</param>
        /// <returns>Whether the numbers are equal with tolerance.</returns>
        /// <example><code>
        /// if (contentHeight.ApproximatelyEquals(position.height))
        ///    return true;
        /// </code></example>
        public static bool ApproximatelyEquals(this float firstNum, float secondNum, float tolerance = 0.01f)
        {
            return System.Math.Abs(firstNum - secondNum) < tolerance;
        }
    } 
}
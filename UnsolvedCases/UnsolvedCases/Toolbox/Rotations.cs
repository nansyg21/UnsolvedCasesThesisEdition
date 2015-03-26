#region File Description
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace UnsolvedCases
{
    public static class Rotations
    {
        /// <summary>
        /// Rotates a point around a specific origin for a specific angle
        /// </summary>
        /// <param name="PointToRotate">The point to rotate</param>
        /// <param name="OriginOfRotation">The origin of the rotation</param>
        /// <param name="ThetaInRads">The angle of the rotation</param>
        /// <returns>The new rotated point</returns>
        public static Vector2 RotatePoint(Vector2 PointToRotate, Vector2 OriginOfRotation, float ThetaInRads)
        {
            Vector2 RotationVector = PointToRotate - OriginOfRotation;
            Vector2 RotatedVector = new Vector2()
            {
                X = (float)(RotationVector.X * Math.Cos(ThetaInRads) - RotationVector.Y * Math.Sin(ThetaInRads)),
                Y = (float)(RotationVector.X * Math.Sin(ThetaInRads) + RotationVector.Y * Math.Cos(ThetaInRads))
            };

            return OriginOfRotation + RotatedVector;
        }
    }
}

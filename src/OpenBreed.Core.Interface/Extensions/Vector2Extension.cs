using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Core.Interface.Extensions
{
    public static class Vector2Extension
    {
        #region Public Methods

        public static float DirectedAngle(this Vector2 first, Vector2 second)
        {
            return (float)(Math.Atan2(second.Y, second.X) - Math.Atan2(first.Y, first.X));
        }

        public static float SmallestAngle(this Vector2 first, Vector2 second)
        {
            var dot = Vector2.Dot(first, second);
            return (float)Math.Acos(MathHelper.Clamp(dot / (first.Length * second.Length), -1.0, 1.0));
        }

        public static Vector2 RotateTowards(this Vector2 current, Vector2 target, float maxRadiansDelta, float maxMagnitudeDelta)
        {
            var magCur = current.Length;
            var magTar = target.Length;

            var f1 = magTar > magCur ? 1.0f : 0.0f;
            var f2 = magCur > magTar ? 1.0f : 0.0f;

            var newMag = magCur + maxMagnitudeDelta * (f1 - f2);
            newMag = Math.Min(newMag, Math.Max(magCur, magTar));
            newMag = Math.Max(newMag, Math.Min(magCur, magTar));

            var totalAngle = current.SmallestAngle(target) - maxRadiansDelta;
            if (totalAngle <= 0)
                return target.Normalized() * newMag;
            else if (totalAngle >= Math.PI)
                return (-target).Normalized() * newMag;

            var axis = current.X * target.Y - current.Y * target.X;
            axis = axis / Math.Abs(axis);
            if (!(1 - Math.Abs(axis) < 0.00001))
                axis = 1;
            current = current.Normalized();
            var newVector = current * (float)Math.Cos(maxRadiansDelta) +
                    new Vector2(-current.Y, current.X) * (float)Math.Sin(maxRadiansDelta) * axis;
            return newVector * newMag;
        }

        public static Vector2 Slerp(this Vector2 a, Vector2 b, float t)
        {
            if (t < 0.0f)
                return a;
            else if (t > 1.0f)
                return b;

            return SlerpUnclamped(a, b, t);
        }

        public static Vector2 SlerpUnclamped(this Vector2 a, Vector2 b, float t)
        {
            var magA = a.Length;
            var magB = b.Length;
            a /= magA;
            b /= magB;
            var dot = Vector2.Dot(a, b);
            dot = System.Math.Max(dot, -1.0f);
            dot = System.Math.Min(dot, 1.0f);
            var theta = System.Math.Acos(dot) * t;
            var relativeVec = (b - a * dot).Normalized();
            var newVec = a * (float)System.Math.Cos(theta) + relativeVec * (float)System.Math.Sin(theta);
            return newVec * (magA + (magB - magA) * t);
        }

        #endregion Public Methods
    }
}
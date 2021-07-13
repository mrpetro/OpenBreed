using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Extensions
{
    public static class Vector3Extension
    {
		public static Vector3 RotateTowards(this Vector3 current, Vector3 target, float maxRadiansDelta, float maxMagnitudeDelta)
		{
			var magCur = current.Length;
			var magTar = target.Length;

			var f1 = magTar > magCur ? 1.0f : 0.0f;
			var f2 = magCur > magTar ? 1.0f : 0.0f;

			var newMag = magCur + maxMagnitudeDelta * (f1 - f2);
			newMag = Math.Min(newMag, Math.Max(magCur, magTar));
			newMag = Math.Max(newMag, Math.Min(magCur, magTar));

			var totalAngle = Vector3.CalculateAngle(current, target) - maxRadiansDelta;
			if (totalAngle <= 0)
				return target.Normalized() * newMag;
			else if (totalAngle >= Math.PI)
				return (-target).Normalized() * newMag;

			var axis = Vector3.Cross(current, target);
			var magAxis = axis.Length;
			if (magAxis == 0)
				axis = Vector3.Cross(current, current + new Vector3(3.95f, 5.32f, -4.24f)).Normalized();
			else
				axis /= magAxis;
			current = current.Normalized();
			var newVector = current * (float)Math.Cos(maxRadiansDelta) +
				Vector3.Cross(axis, current) * (float)Math.Sin(maxRadiansDelta);
			return newVector * newMag;
		}

		public static Vector3 Slerp(this Vector3 a, Vector3 b, float t)
		{
			if (t < 0.0f)
				return a;
			else if (t > 1.0f)
				return b;

			return SlerpUnclamped(a, b, t);
		}

		public static Vector3 SlerpUnclamped(this Vector3 a, Vector3 b, float t)
		{
			var magA = a.Length;
			var magB = b.Length;
			a /= magA;
			b /= magB;
			var dot = Vector3.Dot(a, b);
			dot = System.Math.Max(dot, -1.0f);
			dot = System.Math.Min(dot, 1.0f);
			var theta = System.Math.Acos(dot) * t;
			var relativeVec = (b - a * dot).Normalized();
			var newVec = a * (float)System.Math.Cos(theta) + relativeVec * (float)System.Math.Sin(theta);
			return newVec * (magA + (magB - magA) * t);
		}
	}
}

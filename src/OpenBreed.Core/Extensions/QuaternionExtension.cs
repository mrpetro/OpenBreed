using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Extensions
{
    public static class QuaternionExtension
	{
		public static float Dot(Quaternion q1, Quaternion q2)
		{
			return q1.X * q2.X + q1.Y * q2.Y + q1.Z * q2.Z + q1.W * q2.W;
		}


	}
}

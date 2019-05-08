﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Physics.Helpers
{
    /// <summary>
    /// Extension methods for OpenTK.Box2 class
    /// </summary>
    public static class Box2Extensions
    {
        /// <summary>
        /// Check collision between this box and other box
        /// </summary>
        /// <param name="thisBox">This box</param>
        /// <param name="otherBox">Other box</param>
        /// <returns>True if boxes are colliding, false otherwise</returns>
        public static bool CollidesWith(this Box2 thisBox, Box2 otherBox)
        {
            if (thisBox.Left > otherBox.Right)
                return false;

            if (thisBox.Right < otherBox.Left)
                return false;

            if (thisBox.Bottom > otherBox.Top)
                return false;

            if (thisBox.Top < otherBox.Bottom)
                return false;

            return true;
        }

        /// <summary>
        /// Returns center point of this box
        /// </summary>
        /// <param name="thisBox">This box</param>
        /// <returns>Center point</returns>
        public static Vector2 GetCenter(this Box2 thisBox)
        {
            return new Vector2((thisBox.Left + thisBox.Right) / 2.0f,
                               (thisBox.Bottom + thisBox.Top) / 2.0f);
        }
    }
}

﻿using OpenTK;

namespace OpenBreed.Core.Common.Systems.Components
{
    /// <summary>
    /// Box shape which is always axis aligned (can't be oriented)
    /// </summary>
    public class AxisAlignedBoxShape : IShapeComponent
    {
        #region Private Fields

        private Box2 aabb;

        #endregion Private Fields

        #region Public Constructors

        public AxisAlignedBoxShape()
        {
        }

        #endregion Public Constructors

        #region Private Constructors

        /// <summary>
        /// Constructor for creating shape using x and y coordinates of left bottom corner of box
        /// </summary>
        /// <param name="x">Left coordinate of box</param>
        /// <param name="y">Bottom coordinate of box</param>
        /// <param name="width">Width of box</param>
        /// <param name="height">Height of box</param>
        private AxisAlignedBoxShape(float x, float y, float width, float height)
        {
            aabb = new Box2(0, height, width, 0);
            aabb.Translate(new Vector2(x, y));
        }

        #endregion Private Constructors

        #region Public Properties

        public Box2 Aabb { get { return aabb; } }

        public float Width { get { return Aabb.Width; } }

        public float Height { get { return Aabb.Height; } }

        #endregion Public Properties

        #region Public Methods

        public static AxisAlignedBoxShape Create(float x, float y, float width, float height)
        {
            return new AxisAlignedBoxShape(x, y, width, height);
        }

        #endregion Public Methods
    }
}
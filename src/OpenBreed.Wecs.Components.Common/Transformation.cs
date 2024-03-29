﻿using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Components.Common
{
    public class Transformation
    {
        #region Public Fields

        public Matrix4 Value;

        #endregion Public Fields

        #region Public Constructors

        public Transformation(float x, float y)
        {
            Value = Matrix4.CreateTranslation(x, y, 0.0f);
        }

        public Transformation(Vector2 pos)
        {
            Value = Matrix4.CreateTranslation(pos.X, pos.Y, 0.0f);
        }

        public static Matrix4 CreateTransform(Vector2 pos, float rot, float zoom)
        {
            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-pos.X, -pos.Y, 0));
            transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-rot));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(zoom, zoom, 1.0f));
            return transform;
        }

        public Transformation(Vector2 pos, float rot, float zoom)
        {
            Value = CreateTransform(pos, rot, zoom);
        }

        public Transformation(Matrix4 value)
        {
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public Matrix4 Matrix { get { return Value; } set { Value = value; } }

        #endregion Public Properties
    }
}
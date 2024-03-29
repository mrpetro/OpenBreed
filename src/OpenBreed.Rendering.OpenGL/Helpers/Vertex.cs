﻿using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System.Drawing;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    public struct Vertex
    {
        #region Public Fields
        
        public Vector2 position;
        public Vector2 texCoord;
        public Vector4 color;

        #endregion Public Fields

        #region Public Constructors

        public Vertex(float x, float y)
        {
            this.position = new Vector2(x, y);
            this.texCoord = new Vector2(0,0);
            this.color = new Vector4();
        }

        public Vertex(Vector2 position, Vector2 texCoord, Vector4 color)
        {
            this.position = position;
            this.texCoord = texCoord;
            this.color = color;
        }

        public Vertex(Vector2 position, Vector2 texCoord, Color4 color)
        {
            this.position = position;
            this.texCoord = texCoord;
            this.color = new Vector4(color.R, color.G, color.B, color.A);
        }

        public Vertex(Vector2 position, Vector2 texCoord, Color color)
        {
            this.position = position;
            this.texCoord = texCoord;
            this.color = new Vector4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
        }

        #endregion Public Constructors

        #region Public Properties

        public static int SizeInBytes { get { return Vector2.SizeInBytes * 2 + Vector4.SizeInBytes; } }

        #endregion Public Properties
    }
}
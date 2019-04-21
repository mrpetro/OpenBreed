using OpenBreed.Game.Common;
using OpenBreed.Game.Entities.Components;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace OpenBreed.Game.Rendering
{
    public class MapRenderer : IRenderComponent
    {
        public Type SystemType { get { return typeof(RenderSystem); } }


        #region Public Constructors

        public MapRenderer(int width, int height)
        {
            Width = width;
            Height = height;
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IRenderComponent> Components { get; }

        public int Height { get; }
        public int Width { get; }

        public void Deinitialize(IComponentSystem system)
        {
            throw new System.NotImplementedException();
        }

        #endregion Public Properties

        #region Public Methods

        public void Draw(Viewport viewport)
        {
            GL.Begin(PrimitiveType.Triangles);

            var transf = viewport.View.GetTransform();
            var pointLB = new Vector3(viewport.Left, viewport.Bottom, 0.0f);
            var pointRT = new Vector3(viewport.Right, viewport.Top, 0.0f);

            var tLB = Matrix4.CreateTranslation(pointLB);
            var tRT = Matrix4.CreateTranslation(pointRT);

            tLB = Matrix4.Mult(tLB, transf);
            tRT = Matrix4.Mult(tRT, transf);

            pointLB = tLB.ExtractTranslation();
            pointRT = tRT.ExtractTranslation();

            int leftIndex = (int)pointLB.X / 16;
            int bottomIndex = (int)pointLB.Y / 16;
            int rightIndex = (int)pointRT.X / 16;
            int topIndex = (int)pointRT.Y / 16;

            //Draw static objects first
            for (int j = bottomIndex; j < topIndex; j++)
            {
                for (int i = leftIndex; i < rightIndex; i++)
                {
                    var entity = Components[i + Width * j];

                    //GL.Begin(PrimitiveType.Quads);
                    //GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(entity.Components..Item1.X, triangle.Item1.Y, 0.0f);
                    //GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(triangle.Item2.X, triangle.Item2.Y, 0.0f);
                    //GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(triangle.Item3.X, triangle.Item3.Y, 0.0f);
                    //GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(triangle.Item3.X, triangle.Item3.Y, 0.0f);
                    //GL.End();
                }
            }

            GL.End();
        }

        public void Initialize(IComponentSystem system)
        {
            throw new System.NotImplementedException();
        }

        #endregion Public Methods
    }
}
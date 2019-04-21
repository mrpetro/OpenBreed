using OpenBreed.Game.Common;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Components;
using System;

namespace OpenBreed.Game.Rendering
{
    internal class Sprite : IRenderComponent
    {
        #region Private Fields

        private ITransformComponent transform;

        #endregion Private Fields

        #region Public Constructors

        public Sprite(ITransformComponent transform)
        {
            this.transform = transform;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public Type SystemType { get { return typeof(RenderSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IComponentSystem system)
        {
            throw new System.NotImplementedException();
        }

        public void Draw(Viewport viewport)
        {
            //GL.Begin(PrimitiveType.Quads);
            //GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(entity.Item1.X, triangle.Item1.Y, 0.0f);
            //GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(triangle.Item2.X, triangle.Item2.Y, 0.0f);
            //GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(triangle.Item3.X, triangle.Item3.Y, 0.0f);
            //GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(triangle.Item3.X, triangle.Item3.Y, 0.0f);
            //GL.End();
        }

        public void Initialize(IComponentSystem system)
        {
            throw new System.NotImplementedException();
        }

        #endregion Public Methods
    }
}
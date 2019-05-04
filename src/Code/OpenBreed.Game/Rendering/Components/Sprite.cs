using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Physics.Components;
using OpenBreed.Game.Rendering.Helpers;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Game.Rendering.Components
{
    /// <summary>
    /// Axis-aligned sprite with specific width and height
    /// </summary>
    internal class Sprite : IRenderComponent
    {
        #region Private Fields

        private SpriteAtlas atlas;
        private Position position;

        /// <summary>
        /// For DEBUG Purpose
        /// </summary>
        private DynamicBody body;

        #endregion Private Fields

        #region Public Constructors

        public Sprite(SpriteAtlas atlas)
        {
            this.atlas = atlas;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Id of sprite image from the atlas
        /// </summary>
        public int ImageId { get; set; }

        public Type SystemType { get { return typeof(RenderSystem); } }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        public void Draw(Viewport viewport)
        {
            if(body.Collides)
                GL.Color4(Color4.Red);
            else
                GL.Color4(Color4.Green);

            RenderTools.DrawBox(body.Aabb);

            if (body.Boxes != null)
            {

                GL.Color4(Color4.Yellow);

                foreach (var item in body.Boxes)
                {
                    RenderTools.DrawRectangle(item.Item1 * 16.0f,
                                              item.Item2 * 16.0f,
                                              item.Item1 * 16.0f + 16.0f,
                                              item.Item2 * 16.0f + 16.0f);
                }
            }

            GL.Color4(Color4.White);

            GL.PushMatrix();

            GL.Translate(position.X, position.Y, 0.0f);
            atlas.Draw(viewport, ImageId);

            GL.PopMatrix();
        }

        /// <summary>
        /// Initialize this component
        /// </summary>
        /// <param name="entity">Entity which this component belongs to</param>
        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<Position>().First();
            body = entity.Components.OfType<DynamicBody>().First();
        }

        /// <summary>
        /// Deinitialize this component
        /// </summary>
        /// <param name="entity">Entity which this component belongs to</param>
        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
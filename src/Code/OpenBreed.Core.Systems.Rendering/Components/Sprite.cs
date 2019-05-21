using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Common.Components.Shapes;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Rendering.Helpers;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Core.Systems.Rendering.Components
{
    /// <summary>
    /// Axis-aligned sprite render component
    /// Shared components:
    ///  - axis-aligned box shape
    ///  - position
    /// </summary>
    public class Sprite : ISprite
    {
        #region Private Fields

        private SpriteAtlas atlas;
        private Position position;
        private AxisAlignedBoxShape shape;

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
            GL.PushMatrix();


            GL.Translate((int)position.Current.X, (int)position.Current.Y, 0.0f);
            GL.Translate(- atlas.SpriteSize / 2, - atlas.SpriteSize / 2, 0.0f);
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
            shape = entity.Components.OfType<AxisAlignedBoxShape>().First();
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
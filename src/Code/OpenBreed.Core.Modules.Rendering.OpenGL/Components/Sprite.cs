using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Common.Components.Shapes;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Axis-aligned sprite render component
    /// Shared components:
    ///  - axis-aligned box shape
    ///  - position
    /// </summary>
    internal class Sprite : ISprite
    {
        #region Private Fields

        private ISpriteAtlas atlas;
        private Position position;
        private AxisAlignedBoxShape shape;

        #endregion Private Fields

        #region Internal Constructors

        internal Sprite(ISpriteAtlas atlas, int imageId)
        {
            this.atlas = atlas;
            this.ImageId = ImageId;
        }

        #endregion Internal Constructors

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
        public void Draw(IViewport viewport)
        {
            GL.PushMatrix();

            GL.Translate((int)position.Current.X, (int)position.Current.Y, 0.0f);
            GL.Translate(-atlas.SpriteWidth / 2, -atlas.SpriteHeight / 2, 0.0f);
            atlas.Draw(ImageId);

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
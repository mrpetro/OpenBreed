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

        private Position position;
        private AxisAlignedBoxShape shape;

        #endregion Private Fields

        #region Internal Constructors

        internal Sprite(int atlasId, int imageId)
        {
            AtlasId = atlasId;
            this.ImageId = ImageId;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Speite position component reference
        /// </summary>
        public Position Position { get { return position; } }

        /// <summary>
        /// Id of sprite atlas
        /// </summary>
        public int AtlasId { get; set; }

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
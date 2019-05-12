using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Physics.Components;
using OpenBreed.Core.Systems.Physics.Helpers;
using OpenBreed.Core.Systems.Rendering;
using OpenBreed.Core.Systems.Rendering.Components;
using OpenBreed.Core.Systems.Rendering.Helpers;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Linq;

namespace OpenBreed.Game.Components
{
    public class SpriteDebug : ISprite
    {
        #region Private Fields

        private Position position;
        private DynamicBody body;

        #endregion Private Fields

        #region Public Constructors

        public SpriteDebug()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public Type SystemType { get { return typeof(RenderSystem); } }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        public void Draw(Viewport viewport)
        {
            if (body.Boxes != null)
            {
                foreach (var item in body.Boxes)
                {
                    RenderTools.DrawRectangle(item.Item1 * 16.0f,
                                              item.Item2 * 16.0f,
                                              item.Item1 * 16.0f + 16.0f,
                                              item.Item2 * 16.0f + 16.0f);
                }
            }

            if (body.Collides)
            {
                RenderTools.DrawBox(body.Aabb, Color4.Red);
                RenderTools.DrawLine(body.Aabb.GetCenter(), Vector2.Add(body.Aabb.GetCenter(), body.Projection * 10), Color4.Purple);
            }
            else
                RenderTools.DrawBox(body.Aabb, Color4.Green);
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
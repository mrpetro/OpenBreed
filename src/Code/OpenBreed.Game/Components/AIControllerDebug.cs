using OpenBreed.Core;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems.Common.Components;
using OpenTK.Graphics;
using System;
using System.Linq;

namespace OpenBreed.Game.Components
{
    public class AIControllerDebug : ISprite
    {
        #region Private Fields

        private ISprite sprite;
        private Position position;
        private AICreatureController controller;

        #endregion Private Fields

        #region Public Constructors

        public AIControllerDebug(ISprite sprite)
        {
            this.sprite = sprite ?? throw new ArgumentNullException(nameof(sprite));
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Id of sprite image from the atlas
        /// </summary>
        public int ImageId
        {
            get
            {
                return sprite.ImageId;
            }
            set
            {
                sprite.ImageId = value;
            }
        }

        public Type SystemType { get { return typeof(RenderSystem); } }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        public void Draw(IViewport viewport)
        {
            for (int i = 1; i < controller.Waypoints.Count; i++)
            {
                var wps = controller.Waypoints[i - 1];
                var wpe = controller.Waypoints[i];
                RenderTools.DrawLine(wps, wpe, Color4.Gold);
            }

            if(controller.Waypoints.Count > 0)
                RenderTools.DrawLine(position.Current, controller.Waypoints[0], Color4.Gold);

            sprite.Draw(viewport);
        }

        /// <summary>
        /// Initialize this component
        /// </summary>
        /// <param name="entity">Entity which this component belongs to</param>
        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<Position>().First();
            controller = entity.Components.OfType<AICreatureController>().First();
            sprite.Initialize(entity);
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
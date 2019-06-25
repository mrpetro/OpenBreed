using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Systems.Common.Components;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class SpriteSystem : WorldSystem, IRenderableSystem
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<ISprite> spriteComps = new List<ISprite>();
        private readonly List<IPosition> positionComps = new List<IPosition>();

        #endregion Private Fields

        #region Public Constructors

        public SpriteSystem(ICore core) : base(core)
        {
            Require<ISprite>();
            Require<IPosition>();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            //GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                DrawEntitySprite(viewport, i);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
            spriteComps.Add(entity.Components.OfType<ISprite>().First());
            positionComps.Add(entity.Components.OfType<IPosition>().First());

        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        public void DrawEntitySprite(IViewport viewport, int index)
        {
            var entity = entities[index];
            var sprite = spriteComps[index];
            var position = positionComps[index];

            GL.PushMatrix();

            GL.Translate((int)position.Value.X, (int)position.Value.Y, 0.0f);

            var spriteAtlas = Core.Rendering.Sprites.GetById(sprite.AtlasId);
            GL.Translate(-spriteAtlas.SpriteWidth / 2, -spriteAtlas.SpriteHeight / 2, 0.0f);
            spriteAtlas.Draw(sprite.ImageId);

            GL.PopMatrix();
        }

        #endregion Public Methods
    }
}
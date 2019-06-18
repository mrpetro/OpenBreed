using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems.Common.Components;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class SpriteSystem : ISpriteSystem
    {
        #region Public Fields

        #endregion Public Fields

        #region Private Fields

        private List<ISprite> sprites;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSystem(ICore core)
        {
            Core = core;
            sprites = new List<ISprite>();
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        public void Draw(Viewport viewport)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            //GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < sprites.Count; i++)
                DrawSprite(viewport, sprites[i]);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        /// <summary>
        /// Draw this sprite to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this sprite will be rendered to</param>
        public void DrawSprite(IViewport viewport, ISprite sprite)
        {
            GL.PushMatrix();

            GL.Translate((int)sprite.Position.Value.X, (int)sprite.Position.Value.Y, 0.0f);

            var spriteAtlas = Core.Rendering.Sprites.GetById(sprite.AtlasId);
            GL.Translate(-spriteAtlas.SpriteWidth / 2, -spriteAtlas.SpriteHeight / 2, 0.0f);
            spriteAtlas.Draw(sprite.ImageId);

            GL.PopMatrix();
        }

        public void AddSprite(ISprite sprite)
        {
            sprites.Add(sprite);
        }

        #endregion Public Methods

        #region Private Methods

        public void Initialize(World world)
        {
            throw new NotImplementedException();
        }

        public void Deinitialize(World world)
        {
            throw new NotImplementedException();
        }

        public void Update(float dt)
        {
            throw new NotImplementedException();
        }

        #endregion Private Methods
    }
}
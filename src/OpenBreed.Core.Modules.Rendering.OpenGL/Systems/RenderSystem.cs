using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    /// <summary>
    /// System class that is specialized in rendering
    /// </summary>
    public class RenderSystem : WorldSystem<IRenderComponent>, IRenderSystem
    {
        #region Public Fields

        private TileSystem tiles;
        private SpriteSystem sprites;

        public int MAX_TILES_COUNT = 1024 * 1024;

        #endregion Public Fields

        #region Private Fields

        //private List<ISprite> sprites;

        private List<IDebug> debugs;
        private List<IText> texts;

        #endregion Private Fields

        #region Public Constructors

        public RenderSystem(ICore core, int width, int height, float tileSize) : base(core)
        {
            tiles = new TileSystem(core, width, height, tileSize);
            sprites = new SpriteSystem(core);

            texts = new List<IText>();
            debugs = new List<IDebug>();
        }

        #endregion Public Constructors

        #region Public Properties


        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///This will draw render system objects into given viewport
        /// </summary>
        /// <param name="viewport">Target viewport to draw render system objects</param>
        public void Draw(Viewport viewport)
        {
            tiles.Draw(viewport);

            sprites.Draw(viewport);

            DrawDebugs(viewport);

            DrawTexts(viewport);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void AddComponent(IRenderComponent component)
        {
            if (component is Tile)
                tiles.AddTile((Tile)component);
            else if (component is ISprite)
                sprites.AddSprite((ISprite)component);
            else if (component is IText)
                AddText((IText)component);
            else
                throw new NotImplementedException($"{component}");
        }

        protected override void RemoveComponent(IRenderComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddText(IText text)
        {
            texts.Add(text);
        }

        /// <summary>
        /// This will draw all debugs to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which debugs will be drawn to</param>
        private void DrawDebugs(Viewport viewport)
        {
            for (int i = 0; i < debugs.Count; i++)
                debugs[i].Draw(viewport);
        }

        /// <summary>
        /// Draw all texts to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which sprites will be drawn to</param>
        private void DrawTexts(Viewport viewport)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);
            for (int i = 0; i < texts.Count; i++)
            {
                var text = texts[i];
                text.Draw(viewport);
            }

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }



        #endregion Private Methods
    }
}
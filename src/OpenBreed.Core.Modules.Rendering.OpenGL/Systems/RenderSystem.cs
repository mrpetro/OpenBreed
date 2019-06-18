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
        private TextSystem texts;

        public int MAX_TILES_COUNT = 1024 * 1024;

        #endregion Public Fields

        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public RenderSystem(ICore core, int width, int height, float tileSize) : base(core)
        {
            tiles = new TileSystem(core, width, height, tileSize);
            sprites = new SpriteSystem(core);
            texts = new TextSystem(core);
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

            texts.Draw(viewport);
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
                texts.AddText((IText)component);
            else
                throw new NotImplementedException($"{component}");
        }

        protected override void RemoveComponent(IRenderComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods

        #endregion Private Methods
    }
}
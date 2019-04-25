using OpenBreed.Game.Common;
using OpenBreed.Game.Rendering.Components;
using OpenBreed.Game.Rendering.Helpers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace OpenBreed.Game.Rendering
{
    public class RenderSystem : WorldSystem<IRenderComponent>
    {
        #region Public Fields

        public int MAX_TILES_COUNT = 1024 * 1024;

        #endregion Public Fields

        #region Private Fields

        private List<Sprite> sprites;
        private Tile[] tiles;

        #endregion Private Fields

        #region Public Constructors

        public RenderSystem()
        {
            InitializeTilesMap(64, 64);
            sprites = new List<Sprite>();
        }

        #endregion Public Constructors

        #region Public Properties

        public int TileMapHeight { get; private set; }

        public int TileMapWidth { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public override void AddComponent(IRenderComponent component)
        {
            if (component is Tile)
                AddTile((Tile)component);
            else if (component is Sprite)
                AddSprite((Sprite)component);
            else
                base.AddComponent(component);
        }

        /// <summary>
        ///This will draw render system objects into given viewport
        /// </summary>
        /// <param name="viewport">Target viewport to draw render system objects</param>
        public void Draw(Viewport viewport)
        {
            DrawTiles(viewport);

            foreach (var component in Components)
                component.Draw(viewport);
        }

        #endregion Public Methods

        #region Private Methods

        private void AddSprite(Sprite sprite)
        {
            sprites.Add(sprite);
        }

        private void AddTile(Tile tile)
        {
            int x, y;
            tile.GetMapIndices(out x, out y);
            var tileId = x + TileMapHeight * y;

            if (x >= TileMapWidth)
                throw new InvalidOperationException($"Tile X coordinate exceeds tile map width size.");

            if (y >= TileMapHeight)
                throw new InvalidOperationException($"Tile Y coordinate exceeds tile map height size.");

            tiles[tileId] = tile;
        }

        private void DrawTiles(Viewport viewport)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            int leftIndex = (int)left / 16;
            int bottomIndex = (int)bottom / 16;
            int rightIndex = (int)right / 16 + 1;
            int topIndex = (int)top / 16 + 1;

            leftIndex = MathHelper.Clamp(leftIndex, 0, TileMapHeight);
            rightIndex = MathHelper.Clamp(rightIndex, 0, TileMapHeight);
            bottomIndex = MathHelper.Clamp(bottomIndex, 0, TileMapWidth);
            topIndex = MathHelper.Clamp(topIndex, 0, TileMapWidth);

            GL.Enable(EnableCap.Texture2D);

            for (int j = bottomIndex; j < topIndex; j++)
            {
                for (int i = leftIndex; i < rightIndex; i++)
                {
                    var tile = tiles[i + TileMapHeight * j];

                    if(tile != null)
                        tile.Draw(viewport);
                }
            }

            GL.Disable(EnableCap.Texture2D);
        }

        private void InitializeTilesMap(int width, int height)
        {
            TileMapHeight = width;
            TileMapWidth = height;
            tiles = new Tile[width * height];
        }

        #endregion Private Methods
    }
}
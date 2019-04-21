using OpenBreed.Game.Common;
using OpenBreed.Game.Entities.Components;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace OpenBreed.Game.Rendering
{
    public class RenderSystem : ComponentSystem<IRenderComponent>
    {
        public int MAX_TILES_COUNT = 1024 * 1024;

        #region Private Fields

        private Tile[] tiles;
        private List<Sprite> sprites;

        private List<Viewport> viewports = new List<Viewport>();

        #endregion Private Fields

        #region Public Constructors

        private void InitializeTilesMap(int width, int height)
        {
            TileMapHeight = width;
            TileMapWidth = height;
            tiles = new Tile[width * height];
        }

        public RenderSystem()
        {
            InitializeTilesMap(64, 64);
            sprites = new List<Sprite>();
        }

        #endregion Public Constructors

        #region Public Properties

        public int TileMapWidth { get; private set; }
        public int TileMapHeight { get; private set; }

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

        public void AddViewport(Viewport viewport)
        {
            if (viewports.Contains(viewport))
                throw new InvalidOperationException("Viewport already added.");

            viewports.Add(viewport);
        }

        public void Draw(Viewport viewport)
        {
            DrawTiles(viewport);

            foreach (var component in Components)
                component.Draw(viewport);
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            foreach (var viewport in viewports)
                viewport.Draw(this);

            Cleanup();
        }

        public void RemoveViewport(Viewport viewport)
        {
            viewports.Remove(viewport);
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
            var transf = viewport.View.GetTransform();
            var pointLB = new Vector3(viewport.Left, viewport.Bottom, 0.0f);
            var pointRT = new Vector3(viewport.Right, viewport.Top, 0.0f);

            var tLB = Matrix4.CreateTranslation(pointLB);
            var tRT = Matrix4.CreateTranslation(pointRT);

            tLB = Matrix4.Mult(tLB, transf);
            tRT = Matrix4.Mult(tRT, transf);

            pointLB = tLB.ExtractTranslation();
            pointRT = tRT.ExtractTranslation();

            int leftIndex = (int)pointLB.X / 16;
            int bottomIndex = (int)pointLB.Y / 16;
            int rightIndex = (int)pointRT.X / 16 + 1;
            int topIndex = (int)pointRT.Y / 16 + 1;

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
                    tile.Draw(viewport);
                }
            }

            GL.Disable(EnableCap.Texture2D);
        }

        #endregion Private Methods
    }
}
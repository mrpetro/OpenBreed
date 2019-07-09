using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class TileSystem : WorldSystem, ITileSystem
    {
        #region Public Fields

        public int MAX_TILES_COUNT = 1024 * 1024;

        #endregion Public Fields

        #region Private Fields

        private IEntity[] entities;

        private ITile[] tileComps;

        private IPosition[] positionComps;

        #endregion Private Fields

        #region Public Constructors

        public TileSystem(ICore core, int width, int height, float tileSize, bool gridVisible) : base(core)
        {
            Require<ITile>();
            Require<IPosition>();

            TileSize = tileSize;
            GridVisible = gridVisible;

            InitializeTilesMap(width, height);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Grid visibility flag for debugging purpose
        /// </summary>
        public bool GridVisible { get; set; }

        public int TileMapHeight { get; private set; }
        public int TileMapWidth { get; private set; }
        public float TileSize { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        public void Render(IViewport viewport, float dt)
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

            if (GridVisible)
                DrawGrid(leftIndex, bottomIndex, rightIndex, topIndex);

            GL.Enable(EnableCap.Texture2D);

            for (int j = bottomIndex; j < topIndex; j++)
            {
                for (int i = leftIndex; i < rightIndex; i++)
                {
                    var index = i + TileMapHeight * j;

                    DrawEntityTile(index);
                }
            }

            GL.Disable(EnableCap.Texture2D);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            var position = entity.Components.OfType<IPosition>().First();

            int x, y;
            GetMapIndices(position, out x, out y);
            var tileId = x + TileMapHeight * y;

            if (x >= TileMapWidth)
                throw new InvalidOperationException($"Tile X coordinate exceeds tile map width size.");

            if (y >= TileMapHeight)
                throw new InvalidOperationException($"Tile Y coordinate exceeds tile map height size.");

            entities[tileId] = entity;
            tileComps[tileId] = entity.Components.OfType<ITile>().First();
            positionComps[tileId] = position;
        }

        protected override void UnregisterEntity(IEntity entity)
        {
        }

        #endregion Protected Methods

        #region Private Methods

        private void DrawEntityTile(int index)
        {
            var entity = entities[index];

            if (entity == null)
                return;

            var tile = tileComps[index];
            var position = positionComps[index];

            GL.PushMatrix();

            GL.Translate(position.Value.X, position.Value.Y, 0.0f);

            Core.Rendering.Tiles.GetById(tile.AtlasId).Draw(tile.ImageId);

            GL.PopMatrix();
        }

        private void InitializeTilesMap(int width, int height)
        {
            TileMapHeight = width;
            TileMapWidth = height;
            entities = new IEntity[width * height];
            tileComps = new ITile[width * height];
            positionComps = new IPosition[width * height];
        }

        /// <summary>
        /// Draw debug grid
        /// </summary>
        /// <param name="leftIndex">Left border index of grid</param>
        /// <param name="bottomIndex">Bottom border index of grid</param>
        /// <param name="rightIndex">Right border index of grid</param>
        /// <param name="topIndex">Top border index of grid</param>
        private void DrawGrid(int leftIndex, int bottomIndex, int rightIndex, int topIndex)
        {
            GL.Color4(Color4.Green);

            for (int j = bottomIndex; j < topIndex; j++)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(leftIndex * 16, j * 16);
                GL.Vertex2(rightIndex * 16, j * 16);
                GL.End();
            }

            for (int i = leftIndex; i < rightIndex; i++)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(i * 16, bottomIndex * 16);
                GL.Vertex2(i * 16, topIndex * 16);
                GL.End();
            }
        }

        private void GetMapIndices(IPosition position, out int x, out int y)
        {
            x = (int)(position.Value.X / (int)TileSize);
            y = (int)(position.Value.Y / (int)TileSize);
        }

        #endregion Private Methods
    }
}
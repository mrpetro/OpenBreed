using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class TileSystem : WorldSystem, ITileSystem, IMsgHandler
    {
        #region Public Fields

        public const int TILE_SIZE = 16;
        public int MAX_TILES_COUNT = 1024 * 1024;

        #endregion Public Fields

        #region Private Fields

        private Hashtable entities = new Hashtable();
        private List<ITile>[] cells;

        #endregion Private Fields

        #region Public Constructors

        public TileSystem(ICore core, int width, int height, float tileSize, bool gridVisible) : base(core)
        {
            Require<ITile>();
            Require<GridPosition>();

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

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.MessageBus.RegisterHandler(TileSetMsg.TYPE, this);
        }

        /// <summary>
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            int leftIndex = (int)left / TILE_SIZE;
            int bottomIndex = (int)bottom / TILE_SIZE;
            int rightIndex = (int)right / TILE_SIZE + 1;
            int topIndex = (int)top / TILE_SIZE + 1;

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
                    DrawCellTiles(i, j);
                }
            }

            GL.Disable(EnableCap.Texture2D);
        }

        public override bool HandleMsg(object sender, IMsg msg)
        {
            switch (msg.Type)
            {
                case TileSetMsg.TYPE:
                    return HandleTileSetMsg(sender, (TileSetMsg)msg);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            Debug.Assert(!entities.Contains(entity), "Entity already added!");

            var pos = entity.Components.OfType<GridPosition>().First();

            if (pos.X >= TileMapWidth)
                throw new InvalidOperationException($"Tile X coordinate exceeds tile map width size.");

            if (pos.Y >= TileMapHeight)
                throw new InvalidOperationException($"Tile Y coordinate exceeds tile map height size.");

            var cellIndex = pos.X + TileMapWidth * pos.Y;

            var tile = entity.Components.OfType<ITile>().First();

            entities[entity] = tile;

            var cellTiles = cells[cellIndex];

            if (cellTiles == null)
            {
                cellTiles = new List<ITile>();
                cells[cellIndex] = cellTiles;
            }

            cells[cellIndex].Add(tile);
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleTileSetMsg(object sender, TileSetMsg msg)
        {
            var tile = (ITile)entities[msg.Entity];
            if (tile == null)
                return false;

            tile.ImageId = msg.ImageId;

            return true;
        }

        private void DrawCellTiles(int xIndex, int yIndex)
        {
            var index = xIndex + TileMapWidth * yIndex;

            var cellTiles = cells[index];

            if (cellTiles == null)
                return;

            GL.PushMatrix();

            GL.Translate(xIndex * TILE_SIZE, yIndex * TILE_SIZE, 0.0f);

            for (int i = 0; i < cellTiles.Count; i++)
                Core.Rendering.Tiles.GetById(cellTiles[i].AtlasId).Draw(cellTiles[i].ImageId);

            GL.PopMatrix();
        }

        private void InitializeTilesMap(int width, int height)
        {
            TileMapHeight = width;
            TileMapWidth = height;
            cells = new List<ITile>[width * height];
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
                GL.Vertex2(leftIndex * TILE_SIZE, j * TILE_SIZE);
                GL.Vertex2(rightIndex * TILE_SIZE, j * TILE_SIZE);
                GL.End();
            }

            for (int i = leftIndex; i < rightIndex; i++)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(i * TILE_SIZE, bottomIndex * TILE_SIZE);
                GL.Vertex2(i * TILE_SIZE, topIndex * TILE_SIZE);
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
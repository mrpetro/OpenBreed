using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Core;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class TileSystem : SystemBase, IRenderableSystem
    {
        #region Public Fields

        public const int TILE_SIZE = 16;
        private readonly IEntityMan entityMan;
        private readonly ITileMan tileMan;
        private readonly IStampMan stampMan;
        public int MAX_TILES_COUNT = 1024 * 1024;

        #endregion Public Fields

        #region Private Fields

        private Hashtable entities = new Hashtable();
        private TileCell[] cells;

        #endregion Private Fields

        #region Public Constructors

        internal TileSystem(IEntityMan entityMan, ITileMan tileMan, IStampMan stampMan)
        {
            this.entityMan = entityMan;
            this.tileMan = tileMan;
            this.stampMan = stampMan;
            Require<TileComponent>();
            Require<PositionComponent>();

            //TODO: This can't be constant
            GridWidth = 128;
            GridHeight = 128;
            LayersNo = 1;
            TileSize = 16.0f;
            GridVisible = true;

            InitializeTilesMap();

            RegisterHandler<TileSetCommand>(HandleTileSetCommand);
            RegisterHandler<PutStampCommand>(HandlePutStampCommand);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Grid visibility flag for debugging purpose
        /// </summary>
        public bool GridVisible { get; set; }

        public int GridHeight { get; }
        public int GridWidth { get; }
        public int LayersNo { get; }
        public float TileSize { get; }

        #endregion Public Properties

        #region Public Methods

        public void Render(Box2 clipBox, int depth, float dt)
        {
            ExecuteCommands();

            int leftIndex = (int)clipBox.Left / TILE_SIZE;
            int bottomIndex = (int)clipBox.Bottom / TILE_SIZE;
            int rightIndex = (int)clipBox.Right / TILE_SIZE + 1;
            int topIndex = (int)clipBox.Top / TILE_SIZE + 1;

            leftIndex = MathHelper.Clamp(leftIndex, 0, GridWidth);
            rightIndex = MathHelper.Clamp(rightIndex, 0, GridWidth);
            bottomIndex = MathHelper.Clamp(bottomIndex, 0, GridHeight);
            topIndex = MathHelper.Clamp(topIndex, 0, GridHeight);

            if (GridVisible)
                DrawGrid(leftIndex, bottomIndex, rightIndex, topIndex);

            GL.Enable(EnableCap.Texture2D);

            for (int layerNo = 0; layerNo < LayersNo; layerNo++)
            {
                for (int j = bottomIndex; j < topIndex; j++)
                {
                    for (int i = leftIndex; i < rightIndex; i++)
                    {
                        RenderCellTiles(i, j, layerNo);
                    }
                }
            }

            GL.Disable(EnableCap.Texture2D);
        }

        public bool TryGetGridIndices(Vector2 point, out int xIndex, out int yIndex)
        {
            xIndex = (int)point.X / TILE_SIZE;
            yIndex = (int)point.Y / TILE_SIZE;
            if (xIndex < 0)
                return false;
            if (yIndex < 0)
                return false;
            if (xIndex >= GridWidth)
                return false;
            if (yIndex >= GridHeight)
                return false;

            return true;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            Debug.Assert(!entities.Contains(entity), "Entity already added!");

            var pos = entity.Get<PositionComponent>();

            int xIndex;
            int yIndex;

            if(!TryGetGridIndices(pos.Value, out xIndex, out yIndex))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            var cellIndex = xIndex + GridWidth * yIndex;

            var tile = entity.Get<TileComponent>();

            entities[entity] = tile;

            var tileCell = this.cells[cellIndex];

            tileCell.AtlasId = tile.AtlasId;
            tileCell.ImageId = tile.ImageId;
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleTileSetCommand(TileSetCommand cmd)
        {
            var entity = entityMan.GetById(cmd.EntityId);

            int xIndex;
            int yIndex;

            if (!TryGetGridIndices(cmd.Position, out xIndex, out yIndex))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            var cellIndex = xIndex + GridWidth * yIndex;

            var tileCell = cells[cellIndex];
            tileCell.AtlasId = cmd.AtlasId;
            tileCell.ImageId = cmd.ImageId;

            return true;
        }

        private bool HandlePutStampCommand(PutStampCommand cmd)
        {
            var stamp = stampMan.GetById(cmd.StampId);

            if (stamp == null)
                return false;

            int xIndex;
            int yIndex;

            if (!TryGetGridIndices(cmd.Position, out xIndex, out yIndex))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            for (int j = 0; j < stamp.Height; j++)
            {
                var cellIndex = xIndex + GridWidth * (yIndex + j);

                for (int i = 0; i < stamp.Width; i++)
                {
                    var stampCell = stamp.Cells[i + stamp.Width * j];
                    cells[cellIndex].AtlasId = stampCell.AtlasId;
                    cells[cellIndex].ImageId = stampCell.ImageId;
                    cellIndex++;
                }
            }

            return true;
        }

        private void RenderCellTiles(int xIndex, int yIndex, int layerNo)
        {
            var index = layerNo * GridWidth * GridHeight + xIndex + GridWidth * yIndex;

            var cellTile = cells[index];

            if (!cellTile.IsEmpty)
            {
                GL.PushMatrix();

                GL.Translate(xIndex * TILE_SIZE, yIndex * TILE_SIZE, 0.0f);



                tileMan.GetById(cellTile.AtlasId).Draw(cellTile.ImageId);

                GL.PopMatrix();
            }
        }

        private void InitializeTilesMap()
        {
            cells = new TileCell[GridWidth * GridHeight * LayersNo];
            for (int i = 0; i < cells.Length; i++)
                cells[i] = TileCell.Create();
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

        private void GetMapIndices(PositionComponent position, out int x, out int y)
        {
            x = (int)(position.Value.X / (int)TileSize);
            y = (int)(position.Value.Y / (int)TileSize);
        }

        #endregion Private Methods
    }
}
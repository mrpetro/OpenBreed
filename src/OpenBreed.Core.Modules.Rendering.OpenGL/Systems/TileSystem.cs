using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;

using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Systems;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class TileSystem : WorldSystem, ITileSystem, ICommandExecutor
    {
        #region Public Fields

        public const int TILE_SIZE = 16;
        public int MAX_TILES_COUNT = 1024 * 1024;

        #endregion Public Fields

        #region Private Fields

        private CommandHandler cmdHandler;
        private Hashtable entities = new Hashtable();
        private TileCell[] cells;

        #endregion Private Fields

        #region Public Constructors

        internal TileSystem(TileSystemBuilder builder) : base(builder.core)
        {
            cmdHandler = new CommandHandler(this);

            Require<TileComponent>();
            Require<Position>();

            GridWidth = builder.gridWidth;
            GridHeight = builder.gridHeight;
            LayersNo = builder.layersNo;
            TileSize = builder.tileSize;
            GridVisible = builder.gridVisible;

            InitializeTilesMap();
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

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.RegisterHandler(TileSetCommand.TYPE, cmdHandler);
            World.RegisterHandler(PutStampCommand.TYPE, cmdHandler);
        }

        /// <summary>
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
            cmdHandler.ExecuteEnqueued();

            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            int leftIndex = (int)left / TILE_SIZE;
            int bottomIndex = (int)bottom / TILE_SIZE;
            int rightIndex = (int)right / TILE_SIZE + 1;
            int topIndex = (int)top / TILE_SIZE + 1;

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
                        DrawCellTiles(i, j, layerNo);
                    }
                }
            }

            GL.Disable(EnableCap.Texture2D);
        }

        public override bool ExecuteCommand(object sender, ICommand cmd)
        {
            switch (cmd.Type)
            {
                case TileSetCommand.TYPE:
                    return HandleTileSetCommand(sender, (TileSetCommand)cmd);
                case PutStampCommand.TYPE:
                    return HandlePutStampCommand(sender, (PutStampCommand)cmd);
                default:
                    return false;
            }
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

        protected override void RegisterEntity(IEntity entity)
        {
            Debug.Assert(!entities.Contains(entity), "Entity already added!");

            var pos = entity.Components.OfType<Position>().First();

            int xIndex;
            int yIndex;

            if(!TryGetGridIndices(pos.Value, out xIndex, out yIndex))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            var cellIndex = xIndex + GridWidth * yIndex;

            var tile = entity.Components.OfType<TileComponent>().First();

            entities[entity] = tile;

            var tileCell = this.cells[cellIndex];

            tileCell.AtlasId = tile.AtlasId;
            tileCell.ImageId = tile.ImageId;
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleTileSetCommand(object sender, TileSetCommand cmd)
        {
            int xIndex;
            int yIndex;

            if (!TryGetGridIndices(cmd.Position, out xIndex, out yIndex))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            var cellIndex = xIndex + GridWidth * yIndex;

            var tileCell = this.cells[cellIndex];
            tileCell.AtlasId = cmd.AtlasId;
            tileCell.ImageId = cmd.ImageId;

            return true;
        }

        private bool HandlePutStampCommand(object sender, PutStampCommand msg)
        {
            var stamp = Core.Rendering.Stamps.GetById(msg.StampId);

            if (stamp == null)
                return false;

            int xIndex;
            int yIndex;

            if (!TryGetGridIndices(msg.Position, out xIndex, out yIndex))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            for (int j = 0; j < stamp.Height; j++)
            {
                var cellIndex = xIndex + GridWidth * (yIndex + j);

                for (int i = 0; i < stamp.Width; i++)
                {
                    var imageId = stamp.Data[i + stamp.Width * j];
                    this.cells[cellIndex].ImageId = imageId;
                    this.cells[cellIndex].AtlasId = 0;
                    cellIndex++;
                }
            }

            return true;
        }

        private void DrawCellTiles(int xIndex, int yIndex, int layerNo)
        {
            var index = layerNo * GridWidth * GridHeight + xIndex + GridWidth * yIndex;

            var cellTile = cells[index];

            if (!cellTile.IsEmpty)
            {
                GL.PushMatrix();

                GL.Translate(xIndex * TILE_SIZE, yIndex * TILE_SIZE, 0.0f);

                Core.Rendering.Tiles.GetById(cellTile.AtlasId).Draw(cellTile.ImageId);

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

        private void GetMapIndices(Position position, out int x, out int y)
        {
            x = (int)(position.Value.X / (int)TileSize);
            y = (int)(position.Value.Y / (int)TileSize);
        }

        #endregion Private Methods
    }
}
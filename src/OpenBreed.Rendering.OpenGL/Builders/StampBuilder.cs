using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    internal class StampBuilder : IStampBuilder
    {
        #region Internal Fields

        internal string name;

        internal int width;

        internal int height;

        internal int originX;

        internal int originY;

        internal List<StampCellData> cells = new List<StampCellData>();

        #endregion Internal Fields

        #region Private Fields

        private StampMan manager;

        #endregion Private Fields

        #region Public Constructors

        public StampBuilder(StampMan manager)
        {
            this.manager = manager;
        }

        #endregion Public Constructors

        #region Public Methods

        public ITileStamp Build()
        {
            return new TileStamp(this);
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void SetOrigin(int originX, int originY)
        {
            this.originX = originX;
            this.originY = originY;
        }

        public void ClearTiles()
        {
            cells.Clear();
        }

        public void AddTile(int x, int y, int atlasId, int tileId)
        {
            cells.Add(new StampCellData() { X = x, Y = y, AtlasId = atlasId, TileId = tileId });
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Register(TileStamp newStamp)
        {
            manager.Register(name, newStamp);
        }

        internal int GetId()
        {
            return manager.GenerateNewId();
        }

        internal ITileStampCell[] GetData()
        {
            Debug.Assert(width > 0, "Width must be greater than zero.");
            Debug.Assert(height > 0, "Height must be greater than zero.");

            var data = new ITileStampCell[width * height];

            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];

                Debug.Assert(cell.X >= 0 && cell.X < width, "Cell X must be in range of stamp width.");
                Debug.Assert(cell.Y >= 0 && cell.Y < height, "Cell Y must be in range of stamp height.");

                data[cell.X + width * cell.Y] = new TileStampCell(cell.AtlasId, cell.TileId);
            }

            return data;
        }

        #endregion Internal Methods

        #region Internal Structs

        internal struct StampCellData
        {
            #region Internal Fields

            internal int X;
            internal int Y;
            internal int AtlasId;
            internal int TileId;

            #endregion Internal Fields
        }

        #endregion Internal Structs
    }
}
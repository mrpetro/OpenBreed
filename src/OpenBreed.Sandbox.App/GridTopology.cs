using OpenBreed.Pathfinding.Abstractions.Services;
using OpenBreed.Sandbox.App.Constants;

using System;

namespace OpenBreed.Sandbox.App
{
    internal class GridTopology : ITopology
    {
        #region Public Constructors

        public GridTopology(IDataGrid<CellData> dataGrid)
        {
            DataGrid = dataGrid;
        }

        #endregion Public Constructors

        #region Public Properties

        public IDataGrid<CellData> DataGrid { get; }

        #endregion Public Properties

        #region Public Methods

        public int GetWeight(int id)
        {
            var pos = DataGrid.GetPosition(id);
            var cell = DataGrid.Get(pos);

            switch (cell.GfxId)
            {
                case Tiles.Empty:
                    return 2;

                case Tiles.Water:
                    return 1;

                case Tiles.Wall:
                    return 0;

                default:
                    return 2;
            }
        }

        public bool TryGetNeighborNodeId(int id, int exitId, out int neighbourId)
        {
            switch (exitId)
            {
                case 0:
                    return DataGrid.TryGetLeftFromId(id, out neighbourId);

                case 1:
                    return DataGrid.TryGetUpFromId(id, out neighbourId);

                case 2:
                    return DataGrid.TryGetRightFromId(id, out neighbourId);

                case 3:
                    return DataGrid.TryGetDownFromId(id, out neighbourId);

                default:
                    throw new InvalidOperationException("Supports only 0-3 exit IDs");
            }
        }

        #endregion Public Methods
    }
}
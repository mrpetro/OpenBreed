using OpenBreed.Pathfinding.Abstractions.Services;
using OpenBreed.Sandbox.App.Constants;
using OpenTK.Mathematics;
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

        public bool TryGetNeighborNodeId(int id, int exitId, out int neighbourId, out float distance, out float weight)
        {
            var offset = GetOffset(exitId);
            distance = offset.EuclideanLength;

            var result = DataGrid.TryGetIdByOffset(id, offset, out neighbourId);

            if (!result)
            {
                distance = 0.0f;
                weight = 0.0f;
                return false;
            }

            weight = GetWeight(neighbourId);

            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private float GetWeight(int id)
        {
            var cell = DataGrid.Get(id);

            switch (cell.GfxId)
            {
                case Tiles.Empty:
                    return 1.0f;

                case Tiles.Water:
                    return 0.5f;

                case Tiles.Wall:
                    return 0.0f;

                default:
                    return 1.0f;
            }
        }

        private Vector2i GetOffset(int exitId)
        {
            switch (exitId)
            {
                case 0:
                    return new Vector2i(-1, 0);

                case 1:
                    return new Vector2i(-1, 1);

                case 2:
                    return new Vector2i(0, 1);

                case 3:
                    return new Vector2i(1, 1);

                case 4:
                    return new Vector2i(1, 0);

                case 5:
                    return new Vector2i(1, -1);

                case 6:
                    return new Vector2i(0, -1);

                case 7:
                    return new Vector2i(-1, -1);

                default:
                    throw new InvalidOperationException("Supports only 0-3 exit IDs");
            }
        }

        #endregion Private Methods
    }
}
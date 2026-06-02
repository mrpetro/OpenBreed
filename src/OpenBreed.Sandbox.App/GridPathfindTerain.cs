using OpenBreed.Pathfinding.Abstractions.Services;
using OpenBreed.Sandbox.App.Constants;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.App
{
    internal class GridPathfindTerain : IPathfindTerain
    {
        private readonly IDataGrid<CellData> dataGrid;

        public GridPathfindTerain(IDataGrid<CellData> dataGrid)
        {
            this.dataGrid = dataGrid;
        }

        public int GetId(Vector2i position)
        {
            return dataGrid.GetId(position);
        }

        public Vector2i GetPosition(int id)
        {
            return dataGrid.GetPosition(id);
        }

        public int GetWeight(int id)
        {
            var pos = dataGrid.GetPosition(id);
            var cell = dataGrid.Get(pos);

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

        public bool TryGetDownFromId(int id, out int downId)
        {
           return dataGrid.TryGetDownFromId(id, out downId);
        }

        public bool TryGetLeftFromId(int id, out int leftId)
        {
            return dataGrid.TryGetLeftFromId(id, out leftId);
        }

        public bool TryGetRightFromId(int id, out int rightId)
        {
            return dataGrid.TryGetRightFromId(id, out rightId);
        }

        public bool TryGetUpFromId(int id, out int upId)
        {
            return dataGrid.TryGetUpFromId(id, out upId);
        }
    }
}

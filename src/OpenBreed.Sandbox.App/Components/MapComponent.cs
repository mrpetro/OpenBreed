using OpenBreed.Core.Abstractions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace OpenBreed.Sandbox.App.Components
{
    public class CellData
    {
        #region Public Properties

        public int GfxId { get; set; }

        #endregion Public Properties
    }

    public class MapComponent : IEntityComponent
    {
        #region Public Constructors

        public MapComponent(IDataGrid<CellData> grid, int cellSize)
        {
            Grid = grid;
            CellSize = cellSize;
        }

        #endregion Public Constructors

        #region Public Properties

        public int CellSize { get; }
        public IDataGrid<CellData> Grid { get; }

        public Box2i GetIndices(Box2 box)
        {
            int minIndexX = (int)box.Min.X / CellSize;
            int minIndexY = (int)box.Min.Y / CellSize;
            int maxIndexX = (int)box.Max.X / CellSize + 1;
            int maxIndexY = (int)box.Max.Y / CellSize + 1;

            minIndexX = MathHelper.Clamp(minIndexX, 0, Grid.Width);
            maxIndexX = MathHelper.Clamp(maxIndexX, 0, Grid.Width);
            minIndexY = MathHelper.Clamp(minIndexY, 0, Grid.Height);
            maxIndexY = MathHelper.Clamp(maxIndexY, 0, Grid.Height);
            return new Box2i(minIndexX, minIndexY, maxIndexX, maxIndexY);
        }

        #endregion Public Properties
    }
}
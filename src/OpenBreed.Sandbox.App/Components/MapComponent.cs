using OpenBreed.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public MapComponent(IDataGrid<CellData> grid)
        {
            Grid = grid;
        }

        #endregion Public Constructors

        #region Public Properties

        public IDataGrid<CellData> Grid { get; }

        #endregion Public Properties
    }
}
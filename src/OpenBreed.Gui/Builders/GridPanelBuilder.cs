using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Enums;
using OpenBreed.Gui.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Builders
{
    internal class GridPanelBuilder : ElementBuilder<IGridPanel>, IGridPanelBuilder
    {
        #region Public Constructors

        public GridPanelBuilder()
        {
        }

        #endregion Public Constructors

        #region Internal Properties

        internal List<IGridPartDimension> Columns { get; } = new List<IGridPartDimension>();
        internal List<IGridPartDimension> Rows { get; } = new List<IGridPartDimension>();

        #endregion Internal Properties

        #region Public Methods

        public void AddColumn(float width = -1, DimmensionType dimmensionType = DimmensionType.Size)
        {
            Columns.Add(new GridPartDimension(width, dimmensionType));
        }

        public void AddRow(float height = -1, DimmensionType dimmensionType = DimmensionType.Size)
        {
            Rows.Add(new GridPartDimension(height, dimmensionType));
        }

        public override GridPanel Build()
        {
            return new GridPanel(this);
        }

        #endregion Public Methods
    }
}
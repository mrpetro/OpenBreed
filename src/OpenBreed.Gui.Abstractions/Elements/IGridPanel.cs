using OpenBreed.Gui.Abstractions.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Elements
{
    public interface IGridPanel : IElement
    {
        #region Public Properties

        IReadOnlyList<float> Columns { get; }
        IReadOnlyList<float> Rows { get; }

        IReadOnlyList<IGridPartDimension> ColumnDefinitions { get; }
        IReadOnlyList<IGridPartDimension> RowDefinitions { get; }

        #endregion Public Properties
    }
}
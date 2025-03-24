using OpenBreed.Gui.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Builders
{
    public interface IGridPanelBuilder : IElementBuilder
    {
        #region Public Methods

        void AddColumn(float width = -1, DimmensionType dimmensionType = DimmensionType.Size);

        void AddRow(float height = -1, DimmensionType dimmensionType = DimmensionType.Size);

        #endregion Public Methods
    }
}
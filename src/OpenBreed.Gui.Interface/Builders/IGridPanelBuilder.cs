using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Builders
{
    public enum DimmensionType
    {
        /// <summary>
        /// Size in parent units
        /// </summary>
        Size,
        /// <summary>
        /// Value normalized to parent size.  
        /// </summary>
        SizeNormalized
    }

    public interface IGridPanelBuilder : IContainerBuilder
    {
        #region Public Methods

        void AddColumn(float width = -1, DimmensionType dimmensionType = DimmensionType.Size);

        void AddRow(float height = -1, DimmensionType dimmensionType = DimmensionType.Size);

        #endregion Public Methods
    }
}
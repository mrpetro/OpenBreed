using OpenBreed.Common.Model.Maps.Blocks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps.Layers
{
    public class MapLayerActionVM : MapLayerBaseVM
    {
        #region Private Fields

        private int[] _cells;

        #endregion Private Fields

        #region Internal Constructors

        internal MapLayerActionVM(MapLayoutVM layout) : base(layout)
        {
            _cells = new int[layout.Size.Width * layout.Size.Height];
        }

        #endregion Internal Constructors

        #region Public Methods

        /// <summary>
        /// Get Layer cell using single index
        /// </summary>
        /// <param name="index">Index of cell from list</param>
        /// <returns>Action id</returns>
        public int GetCell(int index)
        {
            return _cells[index];
        }

        public int GetCell(int x, int y)
        {
            return _cells[y * Layout.Size.Width + x];
        }

        public void Restore(MapBodyBlock bodyBlock)
        {
            _cells = new int[bodyBlock.Cells.Length];

            for (int i = 0; i < _cells.Length; i++)
                _cells[i] = bodyBlock.Cells[i].ActionId;
        }

        public void SetCell(int x, int y, int value)
        {
            if (_cells[y * Layout.Size.Width + x] == value)
                return;

            _cells[y * Layout.Size.Width + x] = value;
            Layout.Owner.IsModified = true;
        }

        #endregion Public Methods
    }
}

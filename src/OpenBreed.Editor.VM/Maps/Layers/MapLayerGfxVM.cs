using OpenBreed.Common;
using OpenBreed.Common.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps.Layers
{
    public class MapLayerGfxVM : MapLayerBaseVM
    {
        #region Private Fields

        private TileRef[] _cells;

        #endregion Private Fields

        #region Internal Constructors

        internal MapLayerGfxVM(MapLayoutVM layout) : base(layout)
        {
            _cells = new TileRef[layout.Size.Width * layout.Size.Height];
        }

        #endregion Internal Constructors

        #region Public Methods

        public TileRef GetCell(int x, int y)
        {
            return _cells[y * Layout.Size.Width + x];
        }

        public void Restore(MapBodyDataBlock bodyBlock)
        {
            _cells = new TileRef[bodyBlock.Length];

            for (int i = 0; i < _cells.Length; i++)
                _cells[i] = new TileRef(0, bodyBlock.Cells[i].GfxId);
        }

        public void Store(MapBodyDataBlock bodyBlock)
        {
            for (int i = 0; i < _cells.Length; i++)
                bodyBlock.Cells[i].GfxId = _cells[i].TileId;
        }

        public void SetCell(int x, int y, TileRef value)
        {
            if (_cells[y * Layout.Size.Width + x] == value)
                return;

            _cells[y * Layout.Size.Width + x] = value;
            Layout.Owner.IsModified = true;
        }

        #endregion Public Methods
    }
}

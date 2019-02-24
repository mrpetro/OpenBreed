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

        public override void Restore(IMapLayerModel layerModel)
        {
            var gfxLayerModel = layerModel as MapLayerModel<TileRef>;

            if (gfxLayerModel == null)
                throw new ArgumentException(nameof(layerModel));

            _cells = gfxLayerModel.Cells.ToArray();
        }

        public void Store(IMapLayerModel layerModel)
        {
            var gfxLayerModel = layerModel as MapLayerModel<TileRef>;

            if (gfxLayerModel == null)
                throw new ArgumentException(nameof(layerModel));

            //MapLayoutLayerBuilder<int> builder



            _cells = gfxLayerModel.Cells.ToArray();
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

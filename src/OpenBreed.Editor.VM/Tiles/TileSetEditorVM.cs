using OpenBreed.Common;
using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileSetEditorVM : EntryEditorBaseVM<ITileSetEntry, TileSetVM>
    {

        #region Public Constructors

        public TileSetEditorVM(IRepository repository) : base(repository)
        {
            TileSetViewer = new TileSetViewerVM();

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Tile Set Editor"; } }
        public int SelectedIndex { get; private set; }
        public TileSetViewerVM TileSetViewer { get; }

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry(TileSetVM source, ITileSetEntry target)
        {
            base.UpdateEntry(source, target);
        }

        protected override void UpdateVM(ITileSetEntry source, TileSetVM target)
        {
            var model = DataProvider.GetTileSet(source.Id);

            if (model != null)
            {
                target.TileSize = model.TileSize;
                target.SetupTiles(model.Tiles);
            }

            base.UpdateVM(source, target);
        }

        #endregion Protected Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Editable):
                    TileSetViewer.CurrentTileSet = Editable;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}

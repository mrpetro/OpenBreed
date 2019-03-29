using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Palettes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileSetEditorVM : EntryEditorBaseVM<ITileSetEntry, TileSetVM>
    {
        #region Private Fields

        private string _currentPaletteId;

        #endregion Private Fields

        #region Public Constructors

        public TileSetEditorVM(IRepository repository) : base(repository)
        {
            PaletteIds = new BindingList<string>();
            TileSetViewer = new TileSetViewerVM();
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public string CurrentPaletteId
        {
            get { return _currentPaletteId; }
            set { SetProperty(ref _currentPaletteId, value); }
        }

        public override string EditorName { get { return "Tile Set Editor"; } }

        public BindingList<string> PaletteIds { get; }

        public TileSetViewerVM TileSetViewer { get; }

        #endregion Public Properties

        #region Internal Methods

        internal void SetupPaletteIds(List<string> paletteRefs, TileSetVM target)
        {
            PaletteIds.UpdateAfter(() =>
            {
                PaletteIds.Clear();
                paletteRefs.ForEach(item => PaletteIds.Add(item));
            });

            CurrentPaletteId = PaletteIds.FirstOrDefault();

            SwitchPalette(CurrentPaletteId, target);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void UpdateEntry(TileSetVM source, ITileSetEntry target)
        {
            base.UpdateEntry(source, target);
        }

        protected override void UpdateVM(ITileSetEntry source, TileSetVM target)
        {
            var model = DataProvider.TileSets.GetTileSet(source.Id);

            if (model != null)
            {
                target.TileSize = model.TileSize;
                target.SetupTiles(model.Tiles);
            }

            SetupPaletteIds(source.PaletteRefs, target);
            base.UpdateVM(source, target);
        }

        #endregion Protected Methods

        #region Private Methods

        private void SwitchPalette(string paletteId, TileSetVM target)
        {
            if (target == null)
                return;

            target.Palette = ServiceLocator.Instance.GetService<DataProvider>().Palettes.GetPalette(paletteId);
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentPaletteId):
                    SwitchPalette(CurrentPaletteId, Editable);
                    break;

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
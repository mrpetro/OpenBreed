using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tiles;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Editor.VM.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileSetEditorVM : EntryEditorBaseVM<ITileSetEntry, TileSetViewerVM>
    {
        #region Private Fields

        private string _currentPaletteId = null;
        private int _currentPaletteIndex = -1;

        #endregion Private Fields

        #region Public Constructors

        public TileSetEditorVM(IRepository repository) : base(repository)
        {
            PaletteIds = new BindingList<string>();
            Editable = new TileSetViewerVM();
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public string CurrentPaletteId
        {
            get { return _currentPaletteId; }
            set { SetProperty(ref _currentPaletteId, value); }
        }

        public int CurrentPaletteIndex
        {
            get { return _currentPaletteIndex; }
            set { SetProperty(ref _currentPaletteIndex, value); }
        }

        public override string EditorName { get { return "Tile Set Editor"; } }

        public BindingList<string> PaletteIds { get; }

        #endregion Public Properties

        #region Internal Methods

        internal void SetupPaletteIds(List<string> paletteRefs)
        {
            PaletteIds.UpdateAfter(() =>
            {
                PaletteIds.Clear();
                paletteRefs.ForEach(item => PaletteIds.Add(item));
            });

            CurrentPaletteId = PaletteIds.FirstOrDefault();

            SwitchPalette(CurrentPaletteId);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override TileSetViewerVM CreateVM(ITileSetEntry model)
        {
            return Editable;
        }

        protected override void UpdateEntry(TileSetViewerVM source, ITileSetEntry target)
        {
            base.UpdateEntry(source, target);
        }

        protected override void UpdateVM(ITileSetEntry source, TileSetViewerVM target)
        {
            var model = DataProvider.TileSets.GetTileSet(source.Id);

            if (model != null)
                target.FromModel(model);

            SetupPaletteIds(source.PaletteRefs);
            base.UpdateVM(source, target);
        }

        #endregion Protected Methods

        #region Private Methods

        private void SwitchPalette(string paletteId)
        {
            Editable.Palette = ServiceLocator.Instance.GetService<DataProvider>().Palettes.GetPalette(paletteId);
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentPaletteId):
                    UpdateCurrentPaletteIndex();
                    SwitchPalette(CurrentPaletteId);
                    break;

                case nameof(CurrentPaletteIndex):
                    UpdateCurrentItem();
                    break;

                default:
                    break;
            }
        }

        private void UpdateCurrentItem()
        {
            if (CurrentPaletteIndex == -1)
                CurrentPaletteId = null;
            else
                CurrentPaletteId = PaletteIds[CurrentPaletteIndex];
        }

        private void UpdateCurrentPaletteIndex()
        {
            CurrentPaletteIndex = PaletteIds.IndexOf(CurrentPaletteId);
        }

        #endregion Private Methods
    }
}
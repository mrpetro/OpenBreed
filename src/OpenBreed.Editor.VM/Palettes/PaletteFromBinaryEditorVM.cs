using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromBinaryEditorVM : PaletteEditorExVM, IEntryEditor<IDbPaletteFromBinary>
    {
        #region Public Constructors

        private string _dataRef;

        public PaletteFromBinaryEditorVM(PalettesDataProvider palettesDataProvider,
                                         IModelsProvider dataProvider) : base(palettesDataProvider, dataProvider)
        {
        }
        public string DataRef
        {
            get { return _dataRef; }
            set { SetProperty(ref _dataRef, value); }
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        public override void UpdateVM(IDbPalette entry)
        {
            base.UpdateVM(entry);
            UpdateVM((IDbPaletteFromBinary)entry);
        }

        public override void UpdateEntry(IDbPalette entry)
        {
            base.UpdateEntry(entry);
            UpdateEntry((IDbPaletteFromBinary)entry);
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateVM(IDbPaletteFromBinary entry)
        {
            var model = palettesDataProvider.GetPalette(entry.Id);

            if (model != null)
                UpdateVMColors(model);

            DataRef = entry.DataRef;
        }

        private void UpdateEntry(IDbPaletteFromBinary entry)
        {
            entry.DataRef = DataRef;
        }

        void IEntryEditor<IDbPaletteFromBinary>.UpdateEntry(IDbPaletteFromBinary entry)
        {
            throw new System.NotImplementedException();
        }

        void IEntryEditor<IDbPaletteFromBinary>.UpdateVM(IDbPaletteFromBinary entry)
        {
            throw new System.NotImplementedException();
        }

        #endregion Private Methods
    }
}
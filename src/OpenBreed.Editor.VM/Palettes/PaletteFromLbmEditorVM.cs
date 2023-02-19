using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Model.Palettes;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromLbmEditorVM : PaletteEditorExVM, IEntryEditor<IDbPaletteFromLbm>
    {
        #region Private Fields

        private string _blockName;
        private bool editEnabled;
        private string dataRef;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromLbmEditorVM(PalettesDataProvider palettesDataProvider,
                                      IModelsProvider dataProvider) : base(palettesDataProvider, dataProvider)
        {
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(DataRef):
                    EditEnabled = ValidateSettings();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Public Constructors

        #region Public Properties

        public bool EditEnabled
        {
            get { return editEnabled; }
            set { SetProperty(ref editEnabled, value); }
        }

        #endregion Public Properties

        #region Internal Methods


        public override void UpdateVM(IDbPalette entry)
        {
            base.UpdateVM(entry);
            UpdateVM((IDbPaletteFromLbm)entry);
        }

        public override void UpdateEntry(IDbPalette entry)
        {
            base.UpdateEntry(entry);
            UpdateEntry((IDbPaletteFromLbm)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        public string DataRef
        {
            get { return dataRef; }
            set { SetProperty(ref dataRef, value); }
        }

        private void UpdateVM(IDbPaletteFromLbm entry)
        {
            var model = palettesDataProvider.GetPalette(entry.Id);

            if (model != null)
                UpdateVMColors(model);

            DataRef = entry.DataRef;
        }

        private void UpdateEntry(IDbPaletteFromLbm source)
        {
            var image = dataProvider.GetModel<Image>(DataRef);

            for (int i = 0; i < image.Palette.Entries.Length; i++)
            {
                var color = Colors[i];
                image.Palette.Entries[i] = Color.FromArgb(255, color.R, color.G, color.B);
            }

            source.DataRef = DataRef;
        }

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(DataRef))
                return false;

            return true;
        }

        void IEntryEditor<IDbPaletteFromLbm>.UpdateEntry(IDbPaletteFromLbm entry)
        {
            throw new System.NotImplementedException();
        }

        void IEntryEditor<IDbPaletteFromLbm>.UpdateVM(IDbPaletteFromLbm entry)
        {
            throw new System.NotImplementedException();
        }

        #endregion Private Methods
    }
}
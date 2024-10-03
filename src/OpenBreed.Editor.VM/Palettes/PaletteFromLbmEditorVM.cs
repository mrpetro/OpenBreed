using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
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
    public class PaletteFromLbmEditorVM : PaletteEditorBaseVM<IDbPaletteFromLbm>
    {
        #region Private Fields

        private bool editEnabled;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromLbmEditorVM(
            IDbPaletteFromLbm dbEntry,
            ILogger logger,
            PalettesDataProvider palettesDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, palettesDataProvider, dataProvider, workspaceMan, dialogProvider)
        {
            IgnoreProperty(nameof(EditEnabled));
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName => "LBM Palette Editor";

        public bool EditEnabled
        {
            get { return editEnabled; }
            set { SetProperty(ref editEnabled, value); }
        }

        public string DataRef
        {
            get { return Entry.ImageRef; }
            set { SetProperty(Entry, x => x.ImageRef, value); }
        }

        #endregion Public Properties

        #region Protected Methods

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

        protected override void ProtectedUpdateVM()
        {
            var model = palettesDataProvider.GetPalette(Entry);

            if (model != null)
            {
                UpdateVMColors(model);
            }
        }

        protected override void ProtectedUpdateEntry()
        {
            var image = modelsProvider.GetModel<IDbPaletteFromLbm, Image>(Entry);

            for (int i = 0; i < image.Palette.Entries.Length; i++)
            {
                var color = Colors[i].Color;
                image.Palette.Entries[i] = Color.FromArgb(255, color.R, color.G, color.B);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(DataRef))
                return false;

            return true;
        }

        #endregion Private Methods
    }
}
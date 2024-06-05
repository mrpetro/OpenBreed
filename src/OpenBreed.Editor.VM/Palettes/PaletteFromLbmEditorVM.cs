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

        private string _blockName;
        private bool editEnabled;
        private string dataRef;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromLbmEditorVM(
            ILogger logger,
            PalettesDataProvider palettesDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(logger, palettesDataProvider, dataProvider, workspaceMan, dialogProvider)
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

        public override string EditorName => "LBM Palette Editor";

        public bool EditEnabled
        {
            get { return editEnabled; }
            set { SetProperty(ref editEnabled, value); }
        }

        #endregion Public Properties

        #region Private Methods

        public string DataRef
        {
            get { return dataRef; }
            set { SetProperty(ref dataRef, value); }
        }

        protected override void UpdateVM(IDbPaletteFromLbm entry)
        {
            var model = palettesDataProvider.GetPalette(entry.Id);

            if (model != null)
                UpdateVMColors(model);

            DataRef = entry.DataRef;
        }

        protected override void UpdateEntry(IDbPaletteFromLbm source)
        {
            var image = dataProvider.GetModel<Image>(DataRef);

            for (int i = 0; i < image.Palette.Entries.Length; i++)
            {
                var color = Colors[i].Color;
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

        #endregion Private Methods
    }
}
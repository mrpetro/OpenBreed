using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using System;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromBinaryEditorVM : PaletteEditorBaseVM<IDbPaletteFromBinary>
    {
        #region Public Constructors

        public PaletteFromBinaryEditorVM(
            IDbPaletteFromBinary dbEntry,
            ILogger logger,
            PalettesDataProvider palettesDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, palettesDataProvider, dataProvider, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string DataRef
        {
            get { return Entry.DataRef; }
            set { SetProperty(Entry, x => x.DataRef, value); }
        }

        public int DataStart
        {
            get { return Entry.DataStart; }
            set { SetProperty(Entry, x => x.DataStart, value); }
        }

        public int ColorsNo
        {
            get { return Entry.ColorsNo; }
            set { SetProperty(Entry, x => x.ColorsNo, value); }
        }

        public PaletteMode PaletteMode
        {
            get { return Entry.Mode; }
            set { SetProperty(Entry, x => x.Mode, value); }
        }

        public override string EditorName => "Binary Palette Editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void ProtectedUpdateVM()
        {
            var model = palettesDataProvider.GetPalette(Entry);

            if (model != null)
            {
                UpdateVMColors(model);
            }

            UpdatePalette();
        }

        protected override void ProtectedUpdateEntry()
        {
            Entry.DataRef = DataRef;
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(DataStart):
                case nameof(DataRef):
                    UpdatePalette();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdatePalette()
        {
            var model = palettesDataProvider.GetPalette(Entry);

            if (model != null)
            {
                UpdateVMColors(model);
            }
        }

        #endregion Private Methods
    }
}
using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using System;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromBinaryEditorVM : PaletteEditorBaseVM<IDbPaletteFromBinary>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private string dataRef;
        private int dataStart;
        private int colorsNo;
        private PaletteMode paletteMode;
        private IDbPalette currentEntry;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromBinaryEditorVM(
            ILogger logger,
            PalettesDataProvider palettesDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(logger, palettesDataProvider, dataProvider, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string DataRef
        {
            get { return dataRef; }
            set { SetProperty(ref dataRef, value); }
        }

        public int DataStart
        {
            get { return dataStart; }
            set { SetProperty(ref dataStart, value); }
        }

        public int ColorsNo
        {
            get { return colorsNo; }
            set { SetProperty(ref colorsNo, value); }
        }

        public PaletteMode PaletteMode
        {
            get { return paletteMode; }
            set { SetProperty(ref paletteMode, value); }
        }

        public override string EditorName => "Binary Palette Editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateVM(IDbPaletteFromBinary entry)
        {
            var model = palettesDataProvider.GetPalette(entry.Id);

            if (model != null)
                UpdateVMColors(model);

            dataRef = entry.DataRef;
            dataStart = entry.DataStart;
            paletteMode = entry.Mode;
            colorsNo = entry.ColorsNo;

            UpdatePalette();
        }

        protected override void UpdateEntry(IDbPaletteFromBinary entry)
        {
            entry.DataRef = DataRef;
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
            //var entry = repositoryProvider.GetRepository<IDbAsset>().GetById(DataRef) as IDbPaletteFromBinary;

            //if (entry is null)
            //     return;
            var model = PalettesDataHelper.FromBinary(dataProvider, dataRef, DataStart, colorsNo, PaletteMode);

            if (model != null)
                UpdateVMColors(model);
        }

        #endregion Private Methods
    }
}
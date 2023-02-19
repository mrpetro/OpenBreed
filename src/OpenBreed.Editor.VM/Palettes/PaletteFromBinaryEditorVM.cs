using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using System;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromBinaryEditorVM : PaletteEditorExVM, IEntryEditor<IDbPaletteFromBinary>
    {
        #region Public Constructors

        private string dataRef;
        private int dataStart;
        private int colorsNo;
        private PaletteMode paletteMode;
        private readonly IRepositoryProvider repositoryProvider;

        public PaletteFromBinaryEditorVM(
            PalettesDataProvider palettesDataProvider,
            IRepositoryProvider repositoryProvider,
            IModelsProvider dataProvider) : base(palettesDataProvider, dataProvider)
        {
            this.repositoryProvider = repositoryProvider;
        }

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

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        private IDbPalette currentEntry;

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

            dataRef = entry.DataRef;
            dataStart = entry.DataStart;
            paletteMode = entry.Mode;
            colorsNo = entry.ColorsNo;

            UpdatePalette();
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

        private void UpdatePalette()
        {
            //var entry = repositoryProvider.GetRepository<IDbAsset>().GetById(DataRef) as IDbPaletteFromBinary;

            //if (entry is null)
           //     return;
            var model = PalettesDataHelper.FromBinary(dataProvider, dataRef, DataStart, colorsNo, PaletteMode);

            if (model != null)
                UpdateVMColors(model);


        }
    }
}
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
using System.Linq;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromMapEditorVM : PaletteEditorExVM, IEntryEditor<IDbPaletteFromMap>
    {
        #region Private Fields

        private string _blockName;
        private bool editEnabled;
        private string dataRef;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromMapEditorVM(PalettesDataProvider palettesDataProvider,
                                      IModelsProvider dataProvider) : base(palettesDataProvider, dataProvider)
        {
            BlockNames = new BindingList<string>();
            BlockNames.ListChanged += (s, a) => OnPropertyChanged(nameof(BlockNames));
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(BlockName):
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

        public BindingList<string> BlockNames { get; }

        public string BlockName
        {
            get { return _blockName; }
            set { SetProperty(ref _blockName, value); }
        }

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
            UpdateVM((IDbPaletteFromMap)entry);
        }

        public override void UpdateEntry(IDbPalette entry)
        {
            base.UpdateEntry(entry);
            UpdateEntry((IDbPaletteFromMap)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void UpdatePaletteBlocksList(IDbPaletteFromMap source)
        {
            BlockNames.UpdateAfter(() =>
            {
                BlockNames.Clear();

                var map = dataProvider.GetModel<MapModel>(source.DataRef);

                if (map == null)
                    return;

                foreach (var paletteBlock in map.Blocks.OfType<MapPaletteBlock>())
                    BlockNames.Add(paletteBlock.Name);
            });
        }

        public string DataRef
        {
            get { return dataRef; }
            set { SetProperty(ref dataRef, value); }
        }

        private void UpdateVM(IDbPaletteFromMap entry)
        {
            UpdatePaletteBlocksList(entry);

            var model = palettesDataProvider.GetPalette(entry.Id);

            if (model != null)
                UpdateVMColors(model);

            DataRef = entry.DataRef;
            BlockName = entry.BlockName;
        }

        private void UpdateEntry(IDbPaletteFromMap source)
        {
            var mapModel = dataProvider.GetModel<MapModel>(DataRef);

            var paletteBlock = mapModel.Blocks.OfType<MapPaletteBlock>().FirstOrDefault(item => item.Name == BlockName);

            for (int i = 0; i < paletteBlock.Value.Length; i++)
            {
                var color = Colors[i].Color;
                paletteBlock.Value[i] = new MapPaletteBlock.ColorData(color.R, color.G, color.B);
            }

            source.DataRef = DataRef;
            source.BlockName = BlockName;
        }

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(DataRef))
                return false;

            if (string.IsNullOrWhiteSpace(BlockName))
                return false;

            return true;
        }

        void IEntryEditor<IDbPaletteFromMap>.UpdateEntry(IDbPaletteFromMap entry)
        {
            throw new System.NotImplementedException();
        }

        void IEntryEditor<IDbPaletteFromMap>.UpdateVM(IDbPaletteFromMap entry)
        {
            throw new System.NotImplementedException();
        }

        #endregion Private Methods
    }
}
using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Data;
using OpenBreed.Common.Model.Maps;
using OpenBreed.Common.Model.Maps.Blocks;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Xml.Items.Assets;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromMapVM : PaletteVM
    {

        #region Private Fields

        private string _blockName;
        private bool _editEnabled;

        #endregion Private Fields

        #region Public Constructors

        public PaletteFromMapVM()
        {
            BlockNames = new BindingList<string>();
            BlockNames.ListChanged += (s, a) => OnPropertyChanged(nameof(BlockNames));

            PropertyChanged += This_PropertyChanged;
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
            get { return _editEnabled; }
            set { SetProperty(ref _editEnabled, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IPaletteFromMapEntry)entry);
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IPaletteFromMapEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void UpdatePaletteBlocksList(IPaletteFromMapEntry source)
        {
            BlockNames.UpdateAfter(() =>
            {
                BlockNames.Clear();

                var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

                var map = dataProvider.GetData(source.DataRef) as MapModel;

                if (map == null)
                    return;

                foreach (var paletteBlock in map.Blocks.OfType<MapPaletteBlock>())
                    BlockNames.Add(paletteBlock.Name);

            });
        }

        private void FromEntry(IPaletteFromMapEntry entry)
        {
            UpdatePaletteBlocksList(entry);

            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Palettes.GetPalette(entry.Id);

            if (model != null)
            {
                Colors.UpdateAfter(() =>
                {
                    for (int i = 0; i < model.Data.Length; i++)
                        Colors[i] = model.Data[i];
                });

                CurrentColorIndex = 0;
            }

            DataRef = entry.DataRef;
            BlockName = entry.BlockName;
        }

        private void This_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(BlockName):
                case nameof(DataRef):
                    EditEnabled = ValidateSettings();
                    break;
                default:
                    break;
            }
        }

        private void ToEntry(IPaletteFromMapEntry source)
        {
            var mapModel = ServiceLocator.Instance.GetService<DataProvider>().GetData(DataRef) as MapModel;

            var paletteBlock = mapModel.Blocks.OfType<MapPaletteBlock>().FirstOrDefault(item => item.Name == BlockName);

            for (int i = 0; i < paletteBlock.Value.Length; i++)
            {
                var color = Colors[i];
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

        #endregion Private Methods
    }
}

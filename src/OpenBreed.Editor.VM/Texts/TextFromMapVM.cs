using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Data;
using OpenBreed.Common.Model.Maps;
using OpenBreed.Common.Model.Maps.Blocks;
using OpenBreed.Common.Model.Texts;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Xml.Items.Assets;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextFromMapVM : TextVM
    {

        #region Private Fields

        private string _blockName;
        private bool _editEnabled;

        #endregion Private Fields

        #region Public Constructors

        public TextFromMapVM()
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
            FromEntry((ITextFromMapEntry)entry);
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((ITextFromMapEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void UpdateTextBlocksList(ITextFromMapEntry source)
        {
            BlockNames.UpdateAfter(() =>
            {
                BlockNames.Clear();

                var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

                var map = dataProvider.GetData<MapModel>(source.DataRef);

                if (map == null)
                    return;

                foreach (var textBlock in map.Blocks.OfType<MapTextBlock>())
                    BlockNames.Add(textBlock.Name);

            });
        }

        private void FromEntry(ITextFromMapEntry entry)
        {
            UpdateTextBlocksList(entry);

            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Texts.GetText(entry.Id);

            if (model != null)
                Text = model.Text;

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

        private void ToEntry(ITextFromMapEntry source)
        {
            var mapModel = ServiceLocator.Instance.GetService<DataProvider>().GetData<MapModel>(DataRef);

            var textBlock = mapModel.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == BlockName);

            textBlock.Value = Text;

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

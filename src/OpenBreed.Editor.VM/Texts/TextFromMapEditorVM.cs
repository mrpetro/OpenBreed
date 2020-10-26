using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextFromMapEditorVM : BaseViewModel, IEntryEditor<ITextEntry>
    {
        #region Private Fields

        private string _blockName;
        private bool _editEnabled;

        private string _text;

        private string _dataRef;

        #endregion Private Fields

        #region Public Constructors

        public TextFromMapEditorVM(ParentEntryEditor<ITextEntry> parent)
        {
            Parent = parent;
            BlockNames = new BindingList<string>();
            BlockNames.ListChanged += (s, a) => OnPropertyChanged(nameof(BlockNames));

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public ParentEntryEditor<ITextEntry> Parent { get; }
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

        public string DataRef
        {
            get { return _dataRef; }
            set { SetProperty(ref _dataRef, value); }
        }

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public virtual void UpdateVM(ITextEntry entry)
        {
            var textFromMapEntry = (ITextFromMapEntry)entry;

            UpdateTextBlocksList(textFromMapEntry);

            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Texts.GetText(entry.Id);

            if (model != null)
                Text = model.Text;

            DataRef = textFromMapEntry.DataRef;
            BlockName = textFromMapEntry.BlockName;
        }

        public virtual void UpdateEntry(ITextEntry entry)
        {
            var textFromMapEntry = (ITextFromMapEntry)entry;
            var mapModel = ServiceLocator.Instance.GetService<DataProvider>().GetData<MapModel>(DataRef);

            var textBlock = mapModel.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == BlockName);

            textBlock.Value = Text;

            var model = Parent.DataProvider.Texts.GetText(entry.Id);
            model.Text = Text;

            textFromMapEntry.DataRef = DataRef;
            textFromMapEntry.BlockName = BlockName;
        }

        #endregion Public Methods

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
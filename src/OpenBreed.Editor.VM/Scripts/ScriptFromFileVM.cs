using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Model.Texts;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Editor.VM.Common;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptFromFileVM : ScriptVM
    {
        #region Private Fields

        private bool _editEnabled;

        #endregion Private Fields

        #region Public Constructors

        public ScriptFromFileVM()
        {
            PropertyChanged += This_PropertyChanged;

            ScriptAssetRefIdEditor = new EntryRefIdEditorVM(typeof(IAssetEntry));
            ScriptAssetRefIdEditor.RefIdSelected = (newRefId) => { DataRef = newRefId; };
        }

        #endregion Public Constructors

        #region Public Properties

        public EntryRefIdEditorVM ScriptAssetRefIdEditor { get; }

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
            FromEntry((IScriptFromFileEntry)entry);
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IScriptFromFileEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(IScriptFromFileEntry entry)
        {
            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Scripts.GetScript(entry.Id);

            if (model != null)
                Script = model.Script;

            DataRef = entry.DataRef;
        }

        private void ToEntry(IScriptFromFileEntry source)
        {
            var model = ServiceLocator.Instance.GetService<DataProvider>().GetData<TextModel>(DataRef);

            model.Text = Script;

            source.DataRef = DataRef;
        }

        private void This_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DataRef):
                    EditEnabled = ValidateSettings();
                    ScriptAssetRefIdEditor.RefId = (DataRef == null) ? null : DataRef;
                    break;

                default:
                    break;
            }
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
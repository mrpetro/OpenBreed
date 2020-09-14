using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Model.Texts;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptFromFileEditorVM : BaseViewModel, IEntryEditor<IScriptEntry>
    {
        #region Private Fields

        private bool _editEnabled;

        private string _dataRef;

        private string _script;

        #endregion Private Fields

        #region Public Constructors

        public ScriptFromFileEditorVM(ScriptEditorVM parent)
        {
            Parent = parent;

            PropertyChanged += This_PropertyChanged;

            ScriptAssetRefIdEditor = new EntryRefIdEditorVM(typeof(IAssetEntry));
            ScriptAssetRefIdEditor.RefIdSelected = (newRefId) => { DataRef = newRefId; };
        }

        #endregion Public Constructors

        #region Public Properties

        public ScriptEditorVM Parent { get; }

        public string DataRef
        {
            get { return _dataRef; }
            set { SetProperty(ref _dataRef, value); }
        }

        public EntryRefIdEditorVM ScriptAssetRefIdEditor { get; }

        public bool EditEnabled
        {
            get { return _editEnabled; }
            set { SetProperty(ref _editEnabled, value); }
        }

        public string Script
        {
            get { return _script; }
            set { SetProperty(ref _script, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void UpdateVM(IScriptEntry entry)
        {
            var scriptFromFileEntry = (IScriptFromFileEntry)entry;

            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Scripts.GetScript(entry.Id);

            if (model != null)
                Script = model.Script;

            DataRef = scriptFromFileEntry.DataRef;
        }

        public void UpdateEntry(IScriptEntry entry)
        {
            var scriptFromFileEntry = (IScriptFromFileEntry)entry;

            var model = ServiceLocator.Instance.GetService<DataProvider>().GetData<TextModel>(DataRef);

            model.Text = Script;
            scriptFromFileEntry.DataRef = DataRef;
        }

        #endregion Public Methods

        #region Private Methods

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
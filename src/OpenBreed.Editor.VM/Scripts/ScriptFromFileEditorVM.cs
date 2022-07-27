using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Model.Texts;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Common.Interface.Data;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptFromFileEditorVM : BaseViewModel, IEntryEditor<IDbScript>, IEntryEditor<IDbScriptFromFile>
    {
        #region Private Fields

        private bool _editEnabled;

        private string _dataRef;

        private string _script;
        private readonly ScriptsDataProvider scriptsDataProvider;
        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public ScriptFromFileEditorVM(IWorkspaceMan workspaceMan, ScriptsDataProvider scriptsDataProvider, IModelsProvider dataProvider)
        {
            this.scriptsDataProvider = scriptsDataProvider;
            this.dataProvider = dataProvider;
            PropertyChanged += This_PropertyChanged;

            ScriptAssetRefIdEditor = new EntryRefIdEditorVM(workspaceMan, typeof(IDbAsset));
            ScriptAssetRefIdEditor.RefIdSelected = (newRefId) => { DataRef = newRefId; };
        }

        #endregion Public Constructors

        #region Public Properties

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

        public void UpdateEntry(IDbScriptFromFile entry)
        {
            var scriptFromFileEntry = (IDbScriptFromFile)entry;

            var model = dataProvider.GetModel<TextModel>(DataRef);

            model.Text = Script;
            scriptFromFileEntry.DataRef = DataRef;
        }

        public void UpdateVM(IDbScriptFromFile entry)
        {
            var scriptFromFileEntry = (IDbScriptFromFile)entry;

            var model = scriptsDataProvider.GetScript(entry.Id);

            if (model != null)
                Script = model.Script;

            DataRef = scriptFromFileEntry.DataRef;
        }

        public void UpdateVM(IDbScript entry) => UpdateVM((IDbScriptFromFile)entry);

        public void UpdateEntry(IDbScript entry) => UpdateEntry((IDbScriptFromFile)entry);

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
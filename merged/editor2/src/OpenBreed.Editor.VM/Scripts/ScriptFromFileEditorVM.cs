using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Model.Texts;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using Microsoft.Extensions.Logging;
using OpenBreed.Database.Interface.Items.DataSources;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptFromFileEditorVM : ScriptEditorBaseVM<IDbScriptFromFile>
    {
        #region Private Fields

        private bool _editEnabled;

        private string _dataRef;

        private string _script;
        private readonly ScriptsDataProvider scriptsDataProvider;
        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public ScriptFromFileEditorVM(
            ILogger logger,
            ScriptsDataProvider scriptsDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(logger, workspaceMan, dialogProvider)
        {
            this.scriptsDataProvider = scriptsDataProvider;
            this.dataProvider = dataProvider;
            PropertyChanged += This_PropertyChanged;

            ScriptAssetRefIdEditor = new EntryRefIdEditorVM(
                workspaceMan,
                typeof(IDbDataSource),
                (newRefId) => DataRef = newRefId);
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

        public override string EditorName => "File Script Editor";

        #endregion Public Properties

        #region Public Methods

        protected override void UpdateEntry(IDbScriptFromFile entry)
        {
            var model = scriptsDataProvider.GetScript(entry);

            //if (model is not null)
            //{
            //    Script = model.Script;
            //}

            model.Script = Script;
            entry.DataRef = DataRef;
        }

        protected override void UpdateVM(IDbScriptFromFile entry)
        {
            var model = scriptsDataProvider.GetScript(entry);

            if (model is not null)
            {
                Script = model.Script;
            }

            DataRef = entry.DataRef;
        }

        #endregion Public Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DataRef):
                    EditEnabled = ValidateSettings();
                    ScriptAssetRefIdEditor.CurrentRefId = (DataRef == null) ? null : DataRef;
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
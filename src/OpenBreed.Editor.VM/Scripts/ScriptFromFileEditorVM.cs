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
using OpenBreed.Database.Interface.Items.Palettes;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptFromFileEditorVM : ScriptEditorBaseVM<IDbScriptFromFile>
    {
        #region Private Fields

        private bool editEnabled;

        private string script;
        private readonly ScriptsDataProvider scriptsDataProvider;
        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public ScriptFromFileEditorVM(
            IDbScriptFromFile dbEntry,
            ILogger logger,
            ScriptsDataProvider scriptsDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
            this.scriptsDataProvider = scriptsDataProvider;
            this.dataProvider = dataProvider;

            DataSourceRefIdEditor = new EntryRefIdEditorVM(
                workspaceMan,
                typeof(IDbDataSource),
                (newRefId) => DataRef = newRefId);

            DataSourceRefIdEditor.SelectedRefId = Entry.DataRef;

            UpdateVM();
        }

        #endregion Public Constructors

        #region Public Properties

        public string DataRef
        {
            get { return Entry.DataRef; }
            set { SetProperty(Entry, x => x.DataRef, value); }
        }

        public EntryRefIdEditorVM DataSourceRefIdEditor { get; }

        public bool EditEnabled
        {
            get { return editEnabled; }
            set { SetProperty(ref editEnabled, value); }
        }

        public string Script
        {
            get { return script; }
            set { SetProperty(ref script, value); }
        }

        public override string EditorName => "File Script Editor";

        #endregion Public Properties

        #region Public Methods

        protected override void ProtectedUpdateEntry()
        {
            var model = scriptsDataProvider.GetScript(Entry);

            model.Script = Script;
        }

        protected override void ProtectedUpdateVM()
        {
            var model = scriptsDataProvider.GetScript(Entry);

            if (model is not null)
            {
                Script = model.Script;
            }

            EditEnabled = ValidateSettings();
        }

        #endregion Public Methods

        #region Private Methods



        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(DataRef):
                    DataSourceRefIdEditor.CurrentRefId = (DataRef == null) ? null : DataRef;
                    UpdateVM();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(DataRef))
            {
                return false;
            }

            return true;
        }

        #endregion Private Methods
    }
}
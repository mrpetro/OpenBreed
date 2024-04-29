using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptEmbeddedEditorVM : ScriptEditorBaseVM<IDbScriptEmbedded>
    {
        #region Private Fields

        private readonly ScriptsDataProvider scriptsDataProvider;
        private string dataRef;

        private string script;

        #endregion Private Fields

        #region Public Constructors

        public ScriptEmbeddedEditorVM(
            ScriptsDataProvider scriptsDataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IControlFactory controlFactory) : base(workspaceMan, dialogProvider, controlFactory)
        {
            this.scriptsDataProvider = scriptsDataProvider;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Script
        {
            get { return script; }
            set { SetProperty(ref script, value); }
        }

        public string DataRef
        {
            get { return dataRef; }
            set { SetProperty(ref dataRef, value); }
        }

        public override string EditorName => "Embedded Script Editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry(IDbScriptEmbedded entry)
        {
            entry.DataRef = DataRef;
        }

        protected override void UpdateVM(IDbScriptEmbedded entry)
        {
            var model = scriptsDataProvider.GetScript(entry.Id);

            if (model != null)
                Script = model.Script;

            DataRef = entry.DataRef;
        }

        #endregion Protected Methods
    }
}
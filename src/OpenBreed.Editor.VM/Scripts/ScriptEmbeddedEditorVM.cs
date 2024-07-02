using Microsoft.Extensions.Logging;
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
            ILogger logger,
            ScriptsDataProvider scriptsDataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(logger, workspaceMan, dialogProvider)
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

        public override string EditorName => "Embedded Script Editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry(IDbScriptEmbedded entry)
        {
            entry.Script = Script;
        }

        protected override void UpdateVM(IDbScriptEmbedded entry)
        {
            var model = scriptsDataProvider.GetScript(entry.Id);

            if (model != null)
                Script = model.Script;
        }

        #endregion Protected Methods
    }
}
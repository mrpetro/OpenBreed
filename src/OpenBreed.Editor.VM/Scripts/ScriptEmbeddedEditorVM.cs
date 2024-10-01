using Microsoft.Extensions.Logging;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptEmbeddedEditorVM : ScriptEditorBaseVM<IDbScriptEmbedded>
    {
        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public ScriptEmbeddedEditorVM(
            IDbScriptEmbedded dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string Script
        {
            get { return Entry.Script; }
            set { SetProperty(Entry, x => x.Script, value); }
        }

        public override string EditorName => "Embedded Script Editor";

        #endregion Public Properties
    }
}
using Microsoft.Extensions.Logging;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextEmbeddedEditorVM : TextEditorBaseVM<IDbTextEmbedded>
    {
        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public TextEmbeddedEditorVM(
            IDbTextEmbedded dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string Text
        {
            get { return Entry.Text; }
            set { SetProperty(Entry, x => x.Text, value); }
        }

        public override string EditorName => "Embedded Text Editor";

        #endregion Public Properties
    }
}
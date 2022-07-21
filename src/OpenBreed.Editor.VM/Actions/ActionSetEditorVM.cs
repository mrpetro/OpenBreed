using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Actions;

namespace OpenBreed.Editor.VM.Actions
{
    public class ActionSetEditorVM : ParentEntryEditor<IDbActionSet>
    {
        #region Public Constructors

        public ActionSetEditorVM(DbEntrySubEditorFactory subEditorFactory, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(subEditorFactory, workspaceMan, dialogProvider, "Action Set Editor")
        {
        }

        #endregion Public Constructors
    }
}
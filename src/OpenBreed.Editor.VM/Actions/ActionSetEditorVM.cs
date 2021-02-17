using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Actions;

namespace OpenBreed.Editor.VM.Actions
{
    public class ActionSetEditorVM : ParentEntryEditor<IActionSetEntry>
    {
        #region Public Constructors

        static ActionSetEditorVM()
        {
            RegisterSubeditor<IActionSetEntry, IActionSetEntry>();
        }

        public ActionSetEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dialogProvider, "Action Set Editor")
        {
        }

        #endregion Public Constructors
    }
}
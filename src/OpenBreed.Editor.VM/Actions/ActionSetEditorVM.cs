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
            RegisterSubeditor<IActionSetEntry>((parent) => new ActionSetEmbeddedEditorVM(parent.DataProvider.ActionSets));
        }

        public ActionSetEditorVM(IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(workspaceMan, dataProvider, dialogProvider, "Action Set Editor")
        {
        }

        #endregion Public Constructors
    }
}
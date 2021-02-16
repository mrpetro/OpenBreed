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
            RegisterSubeditor<IActionSetEntry>((workspaceMan, dataProvider, dialogProvider) => new ActionSetEmbeddedEditorVM(dataProvider.ActionSets));
        }

        public ActionSetEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dataProvider, dialogProvider, "Action Set Editor")
        {
        }

        #endregion Public Constructors
    }
}
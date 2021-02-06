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
            RegisterSubeditor<IActionSetEntry>((parent) => new ActionSetEmbeddedEditorVM(parent));
        }

        public ActionSetEditorVM(EditorApplication application, DataProvider dataProvider, IUnitOfWork unitOfWork) : base(application, dataProvider, unitOfWork, "Action Set Editor")
        {
        }

        #endregion Public Constructors
    }
}
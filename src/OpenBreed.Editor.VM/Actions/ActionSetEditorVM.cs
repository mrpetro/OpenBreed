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

        public ActionSetEditorVM(EditorApplication application, IRepository repository) : base(application, repository, "Action Set Editor")
        {
        }

        #endregion Public Constructors
    }
}
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.EntityTemplates;

namespace OpenBreed.Editor.VM.EntityTemplates
{
    public class EntityTemplateEditorVM : ParentEntryEditor<IDbEntityTemplate>
    {
        #region Public Constructors

        public EntityTemplateEditorVM(DbEntrySubEditorFactory subEditorFactory, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(subEditorFactory, workspaceMan, dialogProvider, "Entity Template Editor")
        {
        }

        #endregion Public Constructors
    }
}
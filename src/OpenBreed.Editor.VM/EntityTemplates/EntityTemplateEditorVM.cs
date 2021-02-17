using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.EntityTemplates;

namespace OpenBreed.Editor.VM.EntityTemplates
{
    public class EntityTemplateEditorVM : ParentEntryEditor<IEntityTemplateEntry>
    {
        #region Public Constructors

        static EntityTemplateEditorVM()
        {
            RegisterSubeditor<IEntityTemplateFromFileEntry, IEntityTemplateEntry>();
        }

        public EntityTemplateEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dialogProvider, "Entity Template Editor")
        {
        }

        #endregion Public Constructors
    }
}
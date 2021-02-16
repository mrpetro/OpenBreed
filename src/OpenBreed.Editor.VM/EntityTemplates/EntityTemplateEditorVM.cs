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
            RegisterSubeditor<IEntityTemplateFromFileEntry>((parent) => new EntityTemplateFromFileEditorVM(parent.DataProvider.EntityTemplates,
                                                                                                           parent.DataProvider));
        }

        public EntityTemplateEditorVM(IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(workspaceMan, dataProvider, dialogProvider, "Entity Template Editor")
        {
        }

        #endregion Public Constructors
    }
}
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.EntityTemplates;

namespace OpenBreed.Editor.VM.EntityTemplates
{
    public class EntityTemplateEditorVM : ParentEntryEditor<IEntityTemplateEntry>
    {
        #region Public Constructors

        static EntityTemplateEditorVM()
        {
            RegisterSubeditor<IEntityTemplateFromFileEntry>((parent) => new EntityTemplateFromFileEditorVM(parent));
        }

        public EntityTemplateEditorVM(EditorApplication application, IRepository repository) : base(application, repository, "Entity Template Editor")
        {
        }

        #endregion Public Constructors
    }
}
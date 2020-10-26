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

        public EntityTemplateEditorVM(IRepository repository) : base(repository, "Entity Template Editor")
        {
        }

        #endregion Public Constructors
    }
}
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
            RegisterSubeditor<IEntityTemplateFromFileEntry>((parent) => new EntityTemplateFromFileEditorVM(parent));
        }

        public EntityTemplateEditorVM(EditorApplication application, DataProvider dataProvider) : base(application, dataProvider, "Entity Template Editor")
        {
        }

        #endregion Public Constructors
    }
}
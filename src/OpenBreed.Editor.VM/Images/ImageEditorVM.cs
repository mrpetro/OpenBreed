using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Images;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageEditorVM : ParentEntryEditor<IImageEntry>
    {
        #region Public Constructors

        static ImageEditorVM()
        {
            RegisterSubeditor<IImageEntry>((parent) => new ImageFromFileEditorVM(parent));
        }

        public ImageEditorVM(EditorApplication application, IRepository repository) : base(application, repository, "Image Editor")
        {
        }

        #endregion Public Constructors
    }
}
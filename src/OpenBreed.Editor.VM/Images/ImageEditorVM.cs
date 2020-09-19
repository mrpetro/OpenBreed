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

        public ImageEditorVM(IRepository repository) : base(repository, "Image Editor")
        {
        }

        #endregion Public Constructors
    }
}
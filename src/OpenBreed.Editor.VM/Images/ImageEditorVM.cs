using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Images;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageEditorVM : ParentEntryEditor<IImageEntry>
    {
        #region Public Constructors

        static ImageEditorVM()
        {
            RegisterSubeditor<IImageEntry, IImageEntry>();
        }

        public ImageEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dialogProvider, "Image Editor")
        {
        }

        #endregion Public Constructors
    }
}
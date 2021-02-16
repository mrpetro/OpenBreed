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
            RegisterSubeditor<IImageEntry>((parent) => new ImageFromFileEditorVM(parent.WorkspaceMan,
                                                                                 parent.DialogProvider,
                                                                                 parent.DataProvider));
        }

        public ImageEditorVM(IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(workspaceMan, dataProvider, dialogProvider, "Image Editor")
        {
        }

        #endregion Public Constructors
    }
}
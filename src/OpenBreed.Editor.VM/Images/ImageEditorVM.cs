using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Images;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageEditorVM : ParentEntryEditor<IDbImage>
    {
        #region Public Constructors

        public ImageEditorVM(DbEntrySubEditorFactory subEditorFactory, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(subEditorFactory, workspaceMan, dialogProvider, "Image Editor")
        {
        }

        #endregion Public Constructors
    }
}
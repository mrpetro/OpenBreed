using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Editor.VM.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileSetEditorVM : ParentEntryEditor<ITileSetEntry>
    {
        #region Public Constructors

        public TileSetEditorVM(DbEntrySubEditorFactory subEditorFactory, IWorkspaceMan workspace, IDialogProvider dialogProvider) : base(subEditorFactory, workspace, dialogProvider, "Tile Set Editor")
        {
        }


        #endregion Public Constructors
    }
}
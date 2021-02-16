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

        static TileSetEditorVM()
        {
            RegisterSubeditorEx<ITileSetFromBlkEntry, ITileSetEntry>();
        }

        public TileSetEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspace, DataProvider dataProvider, IDialogProvider dialogProvider) : base(managerCollection, workspace, dataProvider, dialogProvider, "Tile Set Editor")
        {
        }


        #endregion Public Constructors
    }
}
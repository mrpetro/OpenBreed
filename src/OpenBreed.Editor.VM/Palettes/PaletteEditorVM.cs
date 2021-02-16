using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Maps;
using System;
using System.ComponentModel;
using System.Drawing;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteEditorVM : ParentEntryEditor<IPaletteEntry>
    {
        #region Public Constructors

        static PaletteEditorVM()
        {
            RegisterSubeditor<IPaletteFromBinaryEntry>((workspaceMan, dataProvider, dialogProvider) => new PaletteFromBinaryEditorVM(dataProvider.Palettes,
                                                                                                 dataProvider));
            RegisterSubeditor<IPaletteFromMapEntry>((workspaceMan, dataProvider, dialogProvider) => new PaletteFromMapEditorVM(dataProvider.Palettes,
                                                                                           dataProvider));
        }

        public PaletteEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan,dataProvider, dialogProvider, "Palette Editor")
        {
        }

        #endregion Public Constructors
    }
}
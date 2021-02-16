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
            RegisterSubeditor<IPaletteFromBinaryEntry, IPaletteEntry>();
            RegisterSubeditor<IPaletteFromMapEntry, IPaletteEntry>();
        }

        public PaletteEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan,dataProvider, dialogProvider, "Palette Editor")
        {
        }

        #endregion Public Constructors
    }
}
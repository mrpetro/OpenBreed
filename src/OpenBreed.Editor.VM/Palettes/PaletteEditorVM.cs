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
            RegisterSubeditor<IPaletteFromBinaryEntry>((parent) => new PaletteFromBinaryEditorVM(parent.DataProvider.Palettes,
                                                                                                 parent.DataProvider));
            RegisterSubeditor<IPaletteFromMapEntry>((parent) => new PaletteFromMapEditorVM(parent.DataProvider.Palettes,
                                                                                           parent.DataProvider));
        }

        public PaletteEditorVM(IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(workspaceMan,dataProvider, dialogProvider, "Palette Editor")
        {
        }

        #endregion Public Constructors
    }
}
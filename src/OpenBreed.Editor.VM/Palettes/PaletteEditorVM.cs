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
            RegisterSubeditor<IPaletteFromBinaryEntry>((parent) => new PaletteFromBinaryEditorVM(parent));
            RegisterSubeditor<IPaletteFromMapEntry>((parent) => new PaletteFromMapEditorVM(parent));
        }

        public PaletteEditorVM(EditorApplication application, DataProvider dataProvider, IUnitOfWork unitOfWork) : base(application, dataProvider, unitOfWork, "Palette Editor")
        {
        }

        #endregion Public Constructors
    }
}
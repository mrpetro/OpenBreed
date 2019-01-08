using OpenBreed.Common.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Levels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteEditorVM : EntryEditorBaseVM<IPaletteEntry, PaletteVM>
    {
        #region Private Fields

        private Color _currentColor = Color.Empty;
        private int _currentColorIndex = -1;

        #endregion Private Fields

        #region Public Constructors

        public PaletteEditorVM(EditorVM root) : base(root)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public Color CurrentColor
        {
            get { return CurrentColorIndex == -1 ? Color.Empty : Editable.Colors[CurrentColorIndex]; }

            set
            {
                if (Editable.Colors[CurrentColorIndex] == value)
                    return;

                Editable.Colors[CurrentColorIndex] = value;
                OnPropertyChanged(nameof(CurrentColor));
            }
        }

        public int CurrentColorIndex
        {
            get { return _currentColorIndex; }
            set { SetProperty(ref _currentColorIndex, value); }
        }

        public LevelPaletteSelectorVM Palettes { get; private set; }

        public override string EditorName { get { return "Palette Editor"; } }

        protected override void UpdateEntry(PaletteVM source, IPaletteEntry target)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateVM(IPaletteEntry source, PaletteVM target)
        {
            var model = DataProvider.GetPalette(source.Name);
            target.Restore(model);
        }

        #endregion Public Properties

    }
}

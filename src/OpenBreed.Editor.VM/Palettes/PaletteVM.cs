using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Common.Model.Palettes;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteVM : EditableEntryVM
    {

        #region Private Fields

        private Color _currentColor = Color.Empty;
        private int _currentColorIndex = -1;
        private string _dataRef;

        #endregion Private Fields

        #region Public Constructors

        public PaletteVM()
        {
            Colors = new BindingList<Color>();
            Initialize();

            Colors.ListChanged += (s, a) => OnPropertyChanged(nameof(Colors));
        }

        private void Initialize()
        {
            Colors.UpdateAfter(() =>
            {
                for (int i = 0; i < 256; i++)
                    Colors.Add(Color.FromArgb(255, i, i, i));
            });

            CurrentColorIndex = 0;
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<Color> Colors { get; }
        public Color CurrentColor
        {
            get { return CurrentColorIndex == -1 ? Color.Empty : Colors[CurrentColorIndex]; }

            set
            {
                if (Colors[CurrentColorIndex] == value)
                    return;

                Colors[CurrentColorIndex] = value;
                OnPropertyChanged(nameof(CurrentColor));
            }
        }

        public int CurrentColorIndex
        {
            get { return _currentColorIndex; }
            set { SetProperty(ref _currentColorIndex, value); }
        }

        public string DataRef
        {
            get { return _dataRef; }
            set { SetProperty(ref _dataRef, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void FromModel(PaletteModel model)
        {
            Colors.UpdateAfter(() =>
            {
                Colors.Clear();

                foreach (var color in model.Data)
                    Colors.Add(color);
            });
        }

        #endregion Public Methods

        #region Internal Methods

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IPaletteEntry)entry);
        }

        internal virtual void FromEntry(IPaletteEntry entry)
        {
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IPaletteEntry)entry);
        }
        internal virtual void ToEntry(IPaletteEntry entry)
        {
        }

        #endregion Internal Methods

    }
}

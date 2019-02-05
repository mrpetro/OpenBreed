using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteVM : EditableEntryVM
    {

        #region Public Constructors

        public PaletteVM()
        {
            Colors = new BindingList<Color>();
            Colors.ListChanged += (s, a) => OnPropertyChanged(nameof(Colors));
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<Color> Colors { get; }

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

    }
}

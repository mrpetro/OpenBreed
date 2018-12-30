using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Database.Items.Palettes;
using OpenBreed.Common.Sources;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteVM : BaseViewModel
    {
        public EditorVM Root { get; }

        #region Private Fields

        private string _name;

        #endregion Private Fields

        #region Public Constructors

        public PaletteVM(EditorVM root)
        {
            Root = root;

            Colors = new BindingList<Color>();
            Colors.ListChanged += (s, a) => OnPropertyChanged(nameof(Colors));
        }

        internal void Load(string name)
        {
            var model = Root.DataProvider.GetPalette(name);

            Restore(model);

            //NOTE: quick hack to get the result
            Root.LevelEditor.CurrentLevel.Restore(new List<PaletteModel>() { model });
        }


        #endregion Public Constructors

        #region Public Properties

        public BindingList<Color> Colors { get; private set; }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Restore(PaletteModel palette)
        {
            _name = palette.Name;

            Colors.RaiseListChangedEvents = false;
            Colors.Clear();

            foreach (var color in palette.Data)
                Colors.Add(color);

            Colors.RaiseListChangedEvents = true;
            Colors.ResetBindings();
        }

        #endregion Public Methods

    }
}

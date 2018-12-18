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

        internal void Load(PaletteDef paletteDef)
        {
            var sourceDef = Root.Database.GetSourceDef(paletteDef.SourceRef);
            if (sourceDef == null)
                throw new Exception("No Source definition found with name: " + paletteDef.SourceRef);

            var source = Root.SourceMan.GetSource(sourceDef);
            if (source == null)
                throw new Exception("SpriteSet source error: " + sourceDef);

            var model = Root.FormatMan.Load(source, paletteDef.Format) as PaletteModel;
            Restore(model);

            //NOTE: quick hack to get the result
            Root.PaletteViewer.Restore(new List<PaletteModel>() { model });

            Name = paletteDef.Name;
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

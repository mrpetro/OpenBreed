//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;

//namespace OpenBreed.Editor.VM.Palettes
//{
//    public delegate void PaletteColorChangedEventHandler(object sender, PaletteColorChangedEventArgs e);

//    public class PaletteColorChangedEventArgs : EventArgs
//    {
//        public PaletteModel Palette { get; set; }
//        public int Index { get; set; }
//        public Color Color { get; set; }

//        public PaletteColorChangedEventArgs(PaletteModel palette, int index, Color color)
//        {
//            Palette = palette;
//            Index = index;
//            Color = color;
//        }
//    }

//    public delegate void PaletteAddedEventHandler(object sender, PaletteAddedEventArgs e);

//    public class PaletteAddedEventArgs : EventArgs
//    {
//        public PaletteModel Palette { get; set; }

//        public PaletteAddedEventArgs(PaletteModel palette)
//        {
//            Palette = palette;
//        }
//    }

//    public class PalettesModel : ModelBase
//    {
//        public List<PaletteModel> Palettes { get; set; }
//        public event PaletteAddedEventHandler PaletteAdded;
//        public event PaletteColorChangedEventHandler PaletteColorChanged;

//        public PaletteModel DefaultPalette
//        {
//            get
//            {
//                if (Palettes.Count == 0)
//                    return PaletteModel.NullPalette;
//                else
//                    return Palettes.First();
//            }
//        }

//        public PalettesModel()
//        {
//            Palettes = new List<PaletteModel>();
//        }

//        public override void Initialize(EditorModel em, IModelSource source)
//        {
//            base.Initialize(em, source);

//            PaletteModel.NullPalette.Initialize(em, null);
//        }

//        protected virtual void OnPaletteColorChanged(PaletteColorChangedEventArgs e)
//        {
//            if (PaletteColorChanged != null) PaletteColorChanged(this, e);
//        }

//        internal void OnPaletteColorChangedInternal(PaletteColorChangedEventArgs e)
//        {
//            OnPaletteColorChanged(e);
//        }

//        protected virtual void OnPaletteAdded(PaletteAddedEventArgs e)
//        {
//            if (e.Palette.PalettesModel != null)
//                throw new Exception("Palette " + e.Palette + " already added.");

//            e.Palette.PalettesModelInternal = this;
//            if (PaletteAdded != null) PaletteAdded(this, e);
//        }

//        public void Clear()
//        {
//            Palettes.Clear();
//        }

//        public void Add(PaletteModel palette)
//        {
//            Palettes.Add(palette);  
//            OnPaletteAdded(new PaletteAddedEventArgs(palette));
//        }
//    }
//}

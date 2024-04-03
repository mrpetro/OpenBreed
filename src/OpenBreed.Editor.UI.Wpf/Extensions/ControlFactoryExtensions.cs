using OpenBreed.Editor.UI.Wpf.Palettes;
using OpenBreed.Editor.VM.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Wpf.Extensions
{
    public static class ControlFactoryExtensions
    {
        public static void RegisterWpfControls(this ControlFactory controlFactory)
        {
            controlFactory.Register<PaletteFromMapEditorVM, PaletteFromMapCtrl>();
            controlFactory.Register<PaletteFromLbmEditorVM, PaletteFromLbmCtrl>();
            controlFactory.Register<PaletteFromBinaryEditorVM, PaletteFromBinaryCtrl>();
            
        }
    }
}

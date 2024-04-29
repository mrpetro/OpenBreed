using OpenBreed.Editor.UI.Wpf.Actions;
using OpenBreed.Editor.UI.Wpf.DataSources;
using OpenBreed.Editor.UI.Wpf.EntityTemplates;
using OpenBreed.Editor.UI.Wpf.Images;
using OpenBreed.Editor.UI.Wpf.Maps;
using OpenBreed.Editor.UI.Wpf.Palettes;
using OpenBreed.Editor.UI.Wpf.Scripts;
using OpenBreed.Editor.UI.Wpf.Sounds;
using OpenBreed.Editor.UI.Wpf.Sprites;
using OpenBreed.Editor.UI.Wpf.Texts;
using OpenBreed.Editor.UI.Wpf.Tiles;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.DataSources;
using OpenBreed.Editor.VM.EntityTemplates;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Scripts;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Texts;
using OpenBreed.Editor.VM.Tiles;
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
            controlFactory.Register<PcmSoundEditorVM, SoundFromPcmEditorCtrl>();
            controlFactory.Register<ScriptFromFileEditorVM, ScriptFromFileCtrl>();
            controlFactory.Register<ScriptEmbeddedEditorVM, ScriptEmbeddedCtrl>();
            controlFactory.Register<EpfArchiveFileDataSourceEditorVM, EpfArchiveDataSourceCtrl>();
            controlFactory.Register<FileDataSourceEditorVM, FileDataSourceCtrl>();
            controlFactory.Register<ImageFromFileEditorVM, ImageFromFileEditorCtrl>();
            controlFactory.Register<TextEmbeddedEditorVM, TextEmbeddedEditorCtrl>();
            controlFactory.Register<TextFromMapEditorVM, TextFromMapEditorCtrl>();
            controlFactory.Register<ActionSetEmbeddedEditorVM, ActionSetEmbeddedEditorCtrl>();
            controlFactory.Register<EntityTemplateFromFileEditorVM, EntityTemplateFromFileCtrl>();
            controlFactory.Register<TileSetFromBlkEditorVM, TileSetFromBlkEditorCtrl>();
            controlFactory.Register<SpriteSetFromSprEditorVM, SpriteSetFromSprEditorCtrl>();
            controlFactory.Register<SpriteSetFromImageEditorVM, SpriteSetFromImageEditorCtrl>();
            controlFactory.Register<MapEditorVM, MapEditorCtrl>();


        }
    }
}

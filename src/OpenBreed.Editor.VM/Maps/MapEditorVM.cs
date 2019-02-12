using OpenBreed.Editor.VM.Base;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Common;
using OpenBreed.Common.Maps;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorVM : EntryEditorBaseVM<IMapEntry, MapVM>
    {

        #region Public Constructors

        public MapEditorVM(IRepository repository) : base(repository)
        {
            TilesTool = new MapEditorTilesToolVM(this);
            ActionsTool = new MapEditorActionsToolVM(this);
            PaletteSelector = new MapEditorPalettesToolVM(this);

            MapView = new MapEditorViewVM(this);

            PropertyChanged += This_PropertyChanged;

            PaletteSelector.PropertyChanged += PaletteSelector_PropertyChanged;

            //TODO: This is probably bad place for VM connection method
            Connect();
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Level Editor"; } }
        public MapEditorViewVM MapView { get; }
        public MapEditorPalettesToolVM PaletteSelector { get; }
        public MapEditorActionsToolVM ActionsTool { get; }
        public MapEditorTilesToolVM TilesTool { get; }

        #endregion Public Properties

        #region Internal Methods

        internal void Connect()
        {
            ActionsTool.ActionsSelector.PropertyChanged += ActionsSelector_PropertyChanged;

            ActionsTool.Connect();
            TilesTool.Connect();
        }

        private void ActionsSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var actionSelector = sender as MapEditorActionsSelectorVM;

            switch (e.PropertyName)
            {
                case nameof(actionSelector.Items):
                    MapView.Refresh();
                    break;
                default:
                    break;
            }
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void UpdateEntry(MapVM source, IMapEntry target)
        {
            base.UpdateEntry(source, target);
        }

        //    TileSelector.CurrentItem = CurrentLevel.TileSets.FirstOrDefault();
        //    PropSelector.CurrentItem = CurrentLevel.PropSet;
        //    SpriteSetViewer.CurrentItem = CurrentLevel.SpriteSets.FirstOrDefault();
        //    PaletteSelector.CurrentItem = CurrentLevel.Palettes.FirstOrDefault();
        //}
        //protected override void UpdateVM(IMapEntry source, MapVM target)
        //{
        //    target.FromEntry(source);

        //    base.UpdateVM(source, target);
        //}

        #endregion Protected Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Editable):
                    MapView.Layout = Editable.Layout;
                    PaletteSelector.CurrentItem = Editable.Palettes.FirstOrDefault();
                    break;
                default:
                    break;
            }
        }

        private void PaletteSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PaletteSelector.CurrentItem):
                    foreach (var tileSet in Editable.TileSets)
                        tileSet.Palette = PaletteSelector.CurrentItem;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

        //public LevelTileSelectorVM TileSelector { get; }
        //public void Load(string name)
        //{
        //    CurrentLevel = Root.CreateLevel();
        //    //CurrentLevel.Load(name);
        //private void PaletteSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    var paletteSelector = sender as LevelPaletteSelectorVM;

        //    switch (e.PropertyName)
        //    {
        //        case nameof(paletteSelector.CurrentItem):
        //            Root.PaletteEditor.Editable = paletteSelector.CurrentItem;
        //            Root.PaletteEditor.CurrentColorIndex = 0;

        //            foreach (var tileSet in TileSets)
        //                tileSet.Palette = paletteSelector.CurrentItem;

        //            break;
        //        default:
        //            break;
        //    }
        //}

    }
}

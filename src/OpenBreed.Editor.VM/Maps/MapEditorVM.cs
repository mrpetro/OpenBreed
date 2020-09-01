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
using System.ComponentModel;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorVM : EntryEditorBaseVM<IMapEntry, MapVM>
    {

        #region Public Constructors

        public MapEditorVM(IRepository repository) : base(repository)
        {
            Tools = new MapEditorToolsVM();

            TilesTool = new MapEditorTilesToolVM(this);
            ActionsTool = new MapEditorActionsToolVM(this);
            PalettesTool = new MapEditorPalettesToolVM(this);

            MapView = new MapEditorViewVM(this);

            PropertyChanged += This_PropertyChanged;

            PalettesTool.PropertyChanged += PaletteSelector_PropertyChanged;

            ActionsTool.PropertyChanged += ActionsTool_PropertyChanged;

            InitializeTools();

            //TODO: This is probably bad place for VM connection method
            Connect();
        }

        #endregion Public Constructors

        #region Public Properties

        public MapEditorActionsToolVM ActionsTool { get; }
        public override string EditorName { get { return "Level Editor"; } }
        public MapEditorViewVM MapView { get; }
        public MapEditorPalettesToolVM PalettesTool { get; }
        public MapEditorTilesToolVM TilesTool { get; }
        public MapEditorToolsVM Tools { get; }

        #endregion Public Properties

        #region Internal Methods

        internal void Connect()
        {
            ActionsTool.ActionsSelector.PropertyChanged += ActionsSelector_PropertyChanged;

            TilesTool.Connect();
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void UpdateEntry(MapVM source, IMapEntry target)
        {
            base.UpdateEntry(source, target);
        }

        protected override void UpdateVM(IMapEntry source, MapVM target)
        {
            base.UpdateVM(source, target);

            ActionsTool.ActionSet = target.ActionSet;
        }

        #endregion Protected Methods

        #region Private Methods

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

        private void ActionsTool_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ActionsTool.ActionSet):
                    if (Editable != null)
                        Editable.ActionSet = ActionsTool.ActionSet;
                    break;
                default:
                    break;
            }
        }

        private void InitializeTool(MapEditorToolVM tool)
        {

        }

        private void InitializeTools()
        {
            Tools.Items.UpdateAfter(() =>
            {
                Tools.Items.Add(TilesTool);
                Tools.Items.Add(ActionsTool);
                Tools.Items.Add(PalettesTool);
            });

            Tools.CurrentTool = Tools.Items.FirstOrDefault();

            MapView.Cursor.UpdateAction = Tools.OnCursorUpdate;
        }
        private void PaletteSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PalettesTool.CurrentItem):
                    foreach (var tileSet in Editable.TileSets)
                        tileSet.Palette = PalettesTool.CurrentItem;
                    break;
                default:
                    break;
            }
        }

        //    base.UpdateVM(source, target);
        //}
        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Editable):
                    MapView.Layout = Editable.Layout;
                    PalettesTool.CurrentItem = Editable.Palettes.FirstOrDefault();
                    Editable.ActionSet = ActionsTool.ActionSet;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

        //    TileSelector.CurrentItem = CurrentLevel.TileSets.FirstOrDefault();
        //    PropSelector.CurrentItem = CurrentLevel.PropSet;
        //    SpriteSetViewer.CurrentItem = CurrentLevel.SpriteSets.FirstOrDefault();
        //    PaletteSelector.CurrentItem = CurrentLevel.Palettes.FirstOrDefault();
        //}
        //protected override void UpdateVM(IMapEntry source, MapVM target)
        //{
        //    target.FromEntry(source);
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

using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Renderer;
using OpenBreed.Model.Actions;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorVM : EntryEditorBaseVM<IMapEntry>
    {
        #region Private Fields

        private string actionSetRef;

        private string currentPaletteRef;

        private bool isModified;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorVM(IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(workspaceMan, dataProvider, dialogProvider)
        {
            Tools = new MapEditorToolsVM();

            TilesTool = new MapEditorTilesToolVM(this, workspaceMan);
            TilesTool.TilesSelector.ModelChangeAction = OnTileSetModelChange;
            UpdateTileSets = TilesTool.TileSetSelector.UpdateList;
            TilesTool.TileSetSelector.CurrentItemChanged += OnTileSetChanged;

            ActionsTool = new MapEditorActionsToolVM(this, workspaceMan);
            ActionsTool.ModelChangeAction = OnActionSetModelChange;

            PalettesTool = new MapEditorPalettesToolVM(this);
            UpdatePalettes = PalettesTool.UpdateList;
            PalettesTool.ModelChangeAction = OnPalettesModelChange;

            var mapViewRenderTarget = new RenderTarget(1, 1);
            var renderer = new ViewRenderer(this, mapViewRenderTarget);

            MapView = new MapEditorViewVM(this, renderer, mapViewRenderTarget);
            //LayoutVm = new MapLayoutVM(this);
            Properties = new LevelPropertiesVM(this);
            //LayoutVm.PropertyChanged += (s, e) => OnPropertyChanged(nameof(LayoutVm));

            InitializeTools();
        }

        #endregion Public Constructors

        #region Public Properties

        //public MapLayoutVM LayoutVm { get; }
        public Bitmap CurrentTilesBitmap { get; private set; }

        public Action<string> UpdateLayout { get; private set; }
        public Action<string> UpdateTileSets { get; private set; }
        public Action<IEnumerable<string>> UpdatePalettes { get; private set; }

        public MapEditorActionsToolVM ActionsTool { get; }

        public override string EditorName { get { return "Map Editor"; } }

        public MapEditorViewVM MapView { get; }

        public MapEditorPalettesToolVM PalettesTool { get; }

        public MapEditorTilesToolVM TilesTool { get; }

        public MapEditorToolsVM Tools { get; }

        public string ActionSetRef
        {
            get { return actionSetRef; }
            set { SetProperty(ref actionSetRef, value); }
        }

        public string CurrentPaletteRef
        {
            get { return currentPaletteRef; }
            set { SetProperty(ref currentPaletteRef, value); }
        }

        public LevelPropertiesVM Properties { get; }

        internal List<PaletteModel> Palettes => Model.Palettes;
        internal MapLayoutModel Layout => Model.Layout;

        public bool IsModified
        {
            get { return isModified; }
            set { SetProperty(ref isModified, value); }
        }

        internal int TileSize => Model.TileSet.TileSize;

        #endregion Public Properties

        #region Internal Properties

        internal ActionSetModel ActionSet
        {
            get => Model.ActionSet;
            private set => Model.ActionSet = value;
        }

        internal TileSetModel TileSet
        {
            get => Model.TileSet;
            private set => Model.TileSet = value;
        }

        internal MapModel Model { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void RenderDefaultTile(RenderTarget renderTarget, int tileId, float x, float y, int tileSize)
        {
            Font font = new Font("Arial", 5);

            var rectangle = new Rectangle((int)x, (int)y, tileSize, tileSize);

            Color c = Color.Black;
            Pen tileColor = new Pen(c);
            Brush brush = new SolidBrush(c);

            renderTarget.FillRectangle(brush, rectangle);

            c = Color.White;
            tileColor = new Pen(c);
            brush = new SolidBrush(c);

            renderTarget.DrawRectangle(tileColor, rectangle);
            renderTarget.DrawString(string.Format("{0,2:D2}", tileId / 100), font, brush, x + 2, y + 1);
            renderTarget.DrawString(string.Format("{0,2:D2}", tileId % 100), font, brush, x + 2, y + 7);
        }

        public void DrawTile(RenderTarget renderTarget, int tileId, float x, float y, int tileSize)
        {
            if (Model.TileSet == null)
            {
                RenderDefaultTile(renderTarget, tileId, x, y, tileSize);
                return;
            }

            if (tileId >= Model.TileSet.Tiles.Count)
                return;

            var tileRect = Model.TileSet.Tiles[tileId].Rectangle;
            renderTarget.DrawImage(CurrentTilesBitmap, (int)x, (int)y, tileRect);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntry(IMapEntry entry)
        {
            base.UpdateEntry(entry);

            entry.ActionSetRef = ActionSetRef != null ? ActionSetRef : null;

            //LayoutVm.ToMap(Model);
        }

        protected override void UpdateVM(IMapEntry entry)
        {
            base.UpdateVM(entry);

            Model = DataProvider.Maps.GetMap(entry.Id);

            UpdateTileSets(entry.TileSetRef);
            UpdatePalettes(entry.PaletteRefs);

            ActionSetRef = entry.ActionSetRef;
            ActionsTool.CurrentActionSetRef = entry.ActionSetRef;

            //LayoutVm.FromMap(Model);
            Properties.Load(Model);

            TilesTool.UpdateVM();
            ActionsTool.UpdateVM();

            IsModified = false;
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(ActionSetRef):
                    UpdateActionModel();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnTileSetChanged(object sender, string e)
        {
            TilesTool.TilesSelector.CurrentTileSetRef = e;
        }

        private void OnTileSetModelChange(string tileSetRef)
        {
            TileSet = DataProvider.TileSets.GetTileSet(tileSetRef);

            CurrentTilesBitmap = (Bitmap)TileSet.Bitmap.Clone();

            BitmapHelper.SetPaletteColors(CurrentTilesBitmap, Model.Palettes.First().Data);
        }

        private void OnPalettesModelChange(string paletteRef)
        {
            var paletteModel = DataProvider.Palettes.GetPalette(paletteRef);
            BitmapHelper.SetPaletteColors(CurrentTilesBitmap, paletteModel.Data);
        }

        private void OnActionSetModelChange(string tileSetRef)
        {
            ActionSet = DataProvider.ActionSets.GetActionSet(tileSetRef);
        }

        private void UpdateActionModel()
        {
            if (ActionSetRef == null)
            {
                ActionSet = null;
                return;
            }

            var actionSet = DataProvider.ActionSets.GetActionSet(ActionSetRef);
            if (actionSet != null)
                ActionSet = actionSet;
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

        #endregion Private Methods

        //    base.UpdateVM(source, target);
        //}
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
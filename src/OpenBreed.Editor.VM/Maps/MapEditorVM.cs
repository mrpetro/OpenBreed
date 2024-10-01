using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Images;
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
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorVM : EntrySpecificEditorVM<IDbMap>, ITilesToolHost
    {
        #region Private Fields

        private string actionSetRef;

        private string currentPaletteRef;

        private bool isModified;
        private readonly MapsDataProvider mapsDataProvider;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly ActionSetsDataProvider actionSetsDataProvider;
        private readonly TileAtlasDataProvider tileAtlasDataProvider;
        private readonly IDrawingFactory drawingFactory;
        private readonly IBitmapProvider bitmapProvider;
        private readonly IPensProvider pensProvider;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorVM(
            IDbMap dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan, 
            MapsDataProvider mapsDataProvider,
            PalettesDataProvider palettesDataProvider,
            ActionSetsDataProvider actionSetsDataProvider, 
            TileAtlasDataProvider tileAtlasDataProvider, 
            IDialogProvider dialogProvider,
            IDrawingFactory drawingFactory,
            IDrawingContextProvider drawingContextProvider,
            IBitmapProvider bitmapProvider,
            IPensProvider pensProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
            Tools = new MapEditorToolsVM();

            TilesTool = new MapEditorTilesToolVM(this, workspaceMan, drawingFactory);
            TilesTool.TilesSelector.ModelChangeAction = OnTileSetModelChange;
            UpdateTileSets = TilesTool.TileSetSelector.UpdateList;
            TilesTool.TileSetSelector.CurrentItemChanged += OnTileSetChanged;

            ActionsTool = new MapEditorActionsToolVM(this, workspaceMan, drawingFactory, drawingContextProvider);
            ActionsTool.ModelChangeAction = OnActionSetModelChange;

            PalettesTool = new MapEditorPalettesToolVM(this);
            UpdatePalettes = PalettesTool.UpdateList;
            PalettesTool.ModelChangeAction = OnPalettesModelChange;

            var mapViewRenderTarget = drawingFactory.CreateRenderTarget(1, 1);
            var renderer = new ViewRenderer(this, mapViewRenderTarget, pensProvider);

            MapView = new MapEditorViewVM(this, renderer, mapViewRenderTarget, drawingFactory);
            //LayoutVm = new MapLayoutVM(this);
            GeneralProperties = new MapGeneralPropertiesEditorVM(this);
            MissionProperties = new MapMissionPropertiesEditorVM();
            //LayoutVm.PropertyChanged += (s, e) => OnPropertyChanged(nameof(LayoutVm));

            InitializeTools();
            this.mapsDataProvider = mapsDataProvider;
            this.palettesDataProvider = palettesDataProvider;
            this.actionSetsDataProvider = actionSetsDataProvider;
            this.tileAtlasDataProvider = tileAtlasDataProvider;
            this.drawingFactory = drawingFactory;
            this.bitmapProvider = bitmapProvider;
            this.pensProvider = pensProvider;
        }

        #endregion Public Constructors

        #region Public Properties

        //public MapLayoutVM LayoutVm { get; }
        public IImage CurrentTilesBitmap { get; private set; }

        public MySize TileSetSize => new MySize(CurrentTilesBitmap.Width, CurrentTilesBitmap.Height);

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

        public MapGeneralPropertiesEditorVM GeneralProperties { get; }
        public MapMissionPropertiesEditorVM MissionProperties { get; }

        internal List<PaletteModel> Palettes => Model.Palettes;
        public IMapLayoutModel Layout => Model.Layout;

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

        public TileSetModel TileSet
        {
            get => Model.TileSet;
            private set => Model.TileSet = value;
        }

        internal MapModel Model { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void RenderDefaultTile(IRenderTarget renderTarget, int tileId, float x, float y, int tileSize)
        {
            var font = drawingFactory.CreateFont("Arial", 5);

            var rectangle = new MyRectangle((int)x, (int)y, tileSize, tileSize);

            MyColor c = MyColor.Black;
            var tileColor = drawingFactory.CreatePen(c);
            var brush = drawingFactory.CreateSolidBrush(c);

            renderTarget.FillRectangle(brush, rectangle);

            c = MyColor.White;
            tileColor = drawingFactory.CreatePen(c);
            brush = drawingFactory.CreateSolidBrush(c);

            renderTarget.DrawRectangle(tileColor, rectangle);
            renderTarget.DrawString(string.Format("{0,2:D2}", tileId / 100), font, brush, x + 2, y + 1);
            renderTarget.DrawString(string.Format("{0,2:D2}", tileId % 100), font, brush, x + 2, y + 7);
        }

        public void DrawTile(IRenderTarget renderTarget, int tileId, float x, float y, int tileSize)
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

        protected override void UpdateEntry(IDbMap entry)
        {
            base.UpdateEntry(entry);

            entry.ActionSetRef = ActionSetRef != null ? ActionSetRef : null;

            //LayoutVm.ToMap(Model);
        }

        protected override void UpdateVM(IDbMap entry)
        {
            base.UpdateVM(entry);

            Model = mapsDataProvider.GetMap(entry);

            if (Model is null)
            {
                IsModified = false;
                return;
            }

            UpdateTileSets(entry.TileSetRef);
            UpdatePalettes(entry.PaletteRefs);

            ActionSetRef = entry.ActionSetRef;
            ActionsTool.CurrentActionSetRef = entry.ActionSetRef;

            //LayoutVm.FromMap(Model);
            GeneralProperties.Load(Model);
            MissionProperties.Load(Model);


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
            TileSet = tileAtlasDataProvider.GetTileAtlas(tileSetRef);

            CurrentTilesBitmap = bitmapProvider.FromBytes(TileSet.TilesNoX * TileSet.TileSize, TileSet.TilesNoY * TileSet.TileSize, TileSet.Bitmap);

            if (Model.Palettes.Any())
            {
                bitmapProvider.SetPaletteColors(CurrentTilesBitmap, Model.Palettes.First().Data);
            }
        }

        private void OnPalettesModelChange(string paletteRef)
        {
            var paletteModel = palettesDataProvider.GetPalette(paletteRef);
            bitmapProvider.SetPaletteColors(CurrentTilesBitmap, paletteModel.Data);
        }

        private void OnActionSetModelChange(string tileSetRef)
        {
            ActionSet = actionSetsDataProvider.GetActionSet(tileSetRef);
        }

        private void UpdateActionModel()
        {
            if (ActionSetRef == null)
            {
                ActionSet = null;
                return;
            }

            var actionSet = actionSetsDataProvider.GetActionSet(ActionSetRef);
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


    }
}
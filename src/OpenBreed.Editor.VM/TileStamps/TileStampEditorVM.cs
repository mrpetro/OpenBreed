using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.TileStamps
{
    public class TileStampEditorVM : EntrySpecificEditorVM<IDbTileStamp>
    {
        #region Private Fields

        private readonly TileAtlasDataProvider tileSetsDataProvider;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly IFontMan fontMan;
        private readonly IRenderView renderView;
        private string currentPaletteRef = null;

        private TileSetModel model;

        #endregion Private Fields

        #region Public Constructors

        public TileStampEditorVM(
            ILogger logger,
            TileAtlasDataProvider tileSetsDataProvider,
            PalettesDataProvider palettesDataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IDrawingFactory drawingFactory,
            IPrimitiveRenderer primitiveRenderer,
            IFontMan fontMan,
            IDrawingContextProvider drawingContextProvider,
            IBitmapProvider bitmapProvider) : base(logger, workspaceMan, dialogProvider)
        {
            PaletteIds = new ObservableCollection<string>();
            this.tileSetsDataProvider = tileSetsDataProvider;
            this.palettesDataProvider = palettesDataProvider;
            this.primitiveRenderer = primitiveRenderer;
            this.fontMan = fontMan;

            //Viewer = new TileSetViewerVM(drawingFactory, drawingContextProvider, bitmapProvider);

            //OnRender = new DelegateCommand<TimeSpan>((timeSpan) => Render(timeSpan));
           // renderView = renderFactory.CreateView();
            //renderView.Renderer = OnRender;

            //RenderAction = (ts) =>   renderView.Render((float)ts.TotalMilliseconds);
            //ResizeAction = (size) => renderView.Resize(size.Width, size.Height);
            MouseDownCommand = new DelegateCommand<(int, int)>((p) => OnMouseDown(p));
        }

        private (int, int) cursorPos; 

        private void OnMouseDown((int, int) p)
        {
            cursorPos = p;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action InitAction { get; }

        public Action<TimeSpan> RenderAction { get; }

        public Action<(int Width, int Height)> ResizeAction { get; }
        public ICommand MouseDownCommand { get; }
        public ObservableCollection<string> PaletteIds { get; }

        //public TileSetViewerVM Viewer { get; set; }
        public string CurrentPaletteRef
        {
            get { return currentPaletteRef; }
            set { SetProperty(ref currentPaletteRef, value); }
        }

        public override string EditorName => "Tile stamp editor";

        public ICommand RenderCommand { get; set; }

        #endregion Public Properties

        #region Internal Properties

        internal PaletteModel CurrentPalette { get; private set; }

        #endregion Internal Properties

        #region Internal Methods

        internal void SetupPaletteIds(List<string> paletteRefs)
        {
            PaletteIds.Clear();
            paletteRefs.ForEach(item => PaletteIds.Add(item));

            CurrentPaletteRef = PaletteIds.FirstOrDefault();
            //Viewer.Palette = CurrentPalette;
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void UpdateEntry(IDbTileStamp entry)
        {
            base.UpdateEntry(entry);
        }

        protected override void UpdateVM(IDbTileStamp entry)
        {
            base.UpdateVM(entry);
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentPaletteRef):
                    SwitchPalette();
                    //Viewer.Palette = CurrentPalette;
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnStart()
        {
        }

        private void OnRender(IRenderView view, Matrix4 transform, Box2 viewBox, int depth, float dt)
        {
            primitiveRenderer.DrawRectangle(renderView, new Box2(0, 0, 10, 10), Color4.Red, filled: true);

            fontMan.Render(renderView, viewBox, dt, RenderTexts);
        }

        private void RenderTexts(IRenderView view, Box2 clipBox, float dt)
        {
            var cursorPos4 = new Vector4(
                cursorPos.Item1,
                cursorPos.Item2,
                0.0f,
                1.0f);

            cursorPos4 = renderView.GetScreenToWorldCoords(cursorPos4);


            var font = fontMan.GetOSFont("ARIAL", 12);

            fontMan.RenderStart(renderView, new Vector2(200,200));
            fontMan.RenderPart(renderView, font.Id, $"({cursorPos4.X},{cursorPos4.Y})", Vector2.Zero, Color4.Green, 100, clipBox);
            fontMan.RenderEnd(renderView);
        }

        private void SwitchPalette()
        {
            CurrentPalette = palettesDataProvider.GetPalette(CurrentPaletteRef);
        }

        #endregion Private Methods
    }
}
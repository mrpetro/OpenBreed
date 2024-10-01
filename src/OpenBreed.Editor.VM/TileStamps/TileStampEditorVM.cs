using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Tools;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Editor.UI.Mvc.Controllers;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Tiles.Helpers;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Factories;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OpenBreed.Editor.VM.TileStamps
{
    public class TileStampEditorVM : EntrySpecificEditorVM<IDbTileStamp>, ITileStampEditorModel
    {
        #region Private Fields

        private readonly TileAtlasDataProvider tileSetsDataProvider;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IRenderViewFactory renderViewFactory;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private string currentPaletteRef = null;
        private TileStampEditorController renderViewController;

        #endregion Private Fields

        #region Public Constructors

        public TileStampEditorVM(
            IDbTileStamp dbEntry,
            ILogger logger,
            TileAtlasDataProvider tileSetsDataProvider,
            PalettesDataProvider palettesDataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            TilesSelectorVM tilesSelectorVm,
            IRenderViewFactory renderViewFactory,
            IServiceScopeFactory serviceScopeFactory) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
            PaletteIds = new ObservableCollection<string>();
            this.tileSetsDataProvider = tileSetsDataProvider;
            this.palettesDataProvider = palettesDataProvider;
            TilesSelector = tilesSelectorVm;
            this.renderViewFactory = renderViewFactory;
            this.serviceScopeFactory = serviceScopeFactory;

            TilesSelector.TileSetChanged += (s, a) => renderViewController.CurrentTileAtlasId = a;
            TilesSelector.TilesSelectionChanged += (s, a) => renderViewController.CurrentTileSelection = a.Selections;

            IgnoreProperty(nameof(CurrentPaletteRef));
        }

        #endregion Public Constructors

        #region Public Properties

        public Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> InitFunc => OnInitialize;

        public int Width
        {
            get { return Entry.Width; }
            set { SetProperty(Entry, x => x.Width, value); }
        }

        public int Height
        {
            get { return Entry.Height; }
            set { SetProperty(Entry, x => x.Height, value); }
        }

        public int CenterX
        {
            get { return Entry.CenterX; }
            set { SetProperty(Entry, x => x.CenterX, value); }
        }

        public int CenterY
        {
            get { return Entry.CenterY; }
            set { SetProperty(Entry, x => x.CenterY, value); }
        }

        public IReadOnlyList<IDbTileStampCell> Cells => Entry.Cells;

        public TilesSelectorVM TilesSelector { get; }

        public ObservableCollection<string> PaletteIds { get; }

        public string CurrentPaletteRef
        {
            get { return currentPaletteRef; }
            set { SetProperty(ref currentPaletteRef, value); }
        }

        public override string EditorName => "Tile stamp editor";

        #endregion Public Properties

        #region Internal Properties

        internal PaletteModel CurrentPalette { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void EraseTile(Vector4i erasePoint)
        {
            var borderBox = GetBorderBox();

            var tilePos = new MyPoint(erasePoint.X, erasePoint.Y);

            if (!borderBox.ContainsInclusive(new Vector2i(tilePos.X, tilePos.Y)))
            {
                return;
            }

            var foundCell = Entry.Cells.FirstOrDefault(item => item.X == tilePos.X && item.Y == tilePos.Y);

            if (foundCell is not null)
            {
                Entry.RemoveCell(foundCell);
            }

            DataChangedCallback.Invoke();
        }

        public void PutTiles(Vector4i insertPoint, string tileAtlasId, IReadOnlyList<TileSelection> toInsert)
        {
            if (toInsert.Count == 0)
            {
                return;
            }

            var borderBox = GetBorderBox();

            for (int i = 0; i < toInsert.Count; i++)
            {
                var tile = toInsert[i];
                var tilePos = new MyPoint(insertPoint.X + tile.Position.X, insertPoint.Y + tile.Position.Y);

                if (!borderBox.ContainsInclusive(new Vector2i(tilePos.X, tilePos.Y)))
                {
                    continue;
                }

                var foundCell = Entry.Cells.FirstOrDefault(item => item.X == tilePos.X && item.Y == tilePos.Y);

                if (foundCell is null)
                {
                    foundCell = Entry.AddNewCell(tilePos.X, tilePos.Y);
                }

                foundCell.TsId = tileAtlasId;
                foundCell.TsTi = tile.Index;
            }

            DataChangedCallback.Invoke();
        }

        #endregion Public Methods

        #region Internal Methods

        internal void SetupPaletteIds(List<string> paletteRefs)
        {
            PaletteIds.Clear();
            paletteRefs.ForEach(item => PaletteIds.Add(item));

            CurrentPaletteRef = PaletteIds.FirstOrDefault();
        }

        #endregion Internal Methods

        #region Protected Methods

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

        private Box2i GetBorderBox()
        {
            return new Box2i(0, 0, Entry.Width - 1, Entry.Height - 1);
        }

        private void OnModelModified()
        {
        }

        private IRenderContext OnInitialize(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter)
        {
            var serviceScope = serviceScopeFactory.CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<IRenderContextFactory>().SetupScope(hostCoordinateSystemConverter, graphicsContext);

            var renderContext = serviceScope.ServiceProvider.GetRequiredService<IRenderContext>();
            var eventsMan = serviceScope.ServiceProvider.GetRequiredService<IEventsMan>();
            var tileStampDataLoader = serviceScope.ServiceProvider.GetRequiredService<ITileStampDataLoader>();

            var view = new EditorView(eventsMan, renderContext);

            renderViewController = ActivatorUtilities.CreateInstance<TileStampEditorController>(serviceScope.ServiceProvider, view, this);

            if (Entry is not null)
            {
                tileStampDataLoader.Load(Entry);

                eventsMan.Subscribe<RenderContextInitializedEvent>((rc) =>
                {
                    renderViewController.Reset();
                    renderViewController.CurrentTileAtlasId = TilesSelector.CurrentTileSetId;
                });
            }

            return renderContext;
        }

        private void SwitchPalette()
        {
            CurrentPalette = palettesDataProvider.GetPalette(CurrentPaletteRef);
        }

        #endregion Private Methods
    }
}
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
    public class TileStampEditorVM : EntrySpecificEditorVM<IDbTileStamp>
    {
        #region Private Fields

        private readonly TileAtlasDataProvider tileSetsDataProvider;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IRenderViewFactory renderViewFactory;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly TileStampModel model = new TileStampModel();
        private string currentPaletteRef = null;
        private TileStampEditorController renderViewController;

        #endregion Private Fields

        #region Public Constructors

        public TileStampEditorVM(
            ILogger logger,
            TileAtlasDataProvider tileSetsDataProvider,
            PalettesDataProvider palettesDataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            TilesSelectorVM tilesSelectorVm,
            IRenderViewFactory renderViewFactory,
            IServiceScopeFactory serviceScopeFactory) : base(logger, workspaceMan, dialogProvider)
        {
            PaletteIds = new ObservableCollection<string>();
            this.tileSetsDataProvider = tileSetsDataProvider;
            this.palettesDataProvider = palettesDataProvider;
            TilesSelector = tilesSelectorVm;
            this.renderViewFactory = renderViewFactory;
            this.serviceScopeFactory = serviceScopeFactory;

            TilesSelector.TileSetChanged += (s, a) => renderViewController.CurrentTileAtlasId = a;
            TilesSelector.TilesSelectionChanged += (s, a) => renderViewController.CurrentTileSelection = a.Selections;
        }

        #endregion Public Constructors

        #region Public Properties

        public Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> InitFunc => OnInitialize;

        public int StampWidth
        {
            get { return Entry.Width; }
            set { SetProperty(Entry, x => x.Width, value); }
        }

        public int StampHeight
        {
            get { return Entry.Height; }
            set { SetProperty(Entry, x => x.Height, value); }
        }

        public int StampCenterX
        {
            get { return Entry.CenterX; }
            set { SetProperty(Entry, x => x.CenterX, value); }
        }

        public int StampCenterY
        {
            get { return Entry.CenterY; }
            set { SetProperty(Entry, x => x.CenterY, value); }
        }

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

        #region Internal Methods

        internal void SetupPaletteIds(List<string> paletteRefs)
        {
            PaletteIds.Clear();
            paletteRefs.ForEach(item => PaletteIds.Add(item));

            CurrentPaletteRef = PaletteIds.FirstOrDefault();
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

            model.Load(entry);
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

        private IRenderContext OnInitialize(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter)
        {
            var serviceScope = serviceScopeFactory.CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<IRenderContextFactory>().SetupScope(hostCoordinateSystemConverter, graphicsContext);

            var renderContext = serviceScope.ServiceProvider.GetRequiredService<IRenderContext>();
            var eventsMan = serviceScope.ServiceProvider.GetRequiredService<IEventsMan>();
            var tileStampDataLoader = serviceScope.ServiceProvider.GetRequiredService<ITileStampDataLoader>();

            var view = new EditorView(eventsMan, renderContext);

            renderViewController = ActivatorUtilities.CreateInstance<TileStampEditorController>(serviceScope.ServiceProvider, view, model);

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
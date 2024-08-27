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
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Events;
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
        private readonly ITileStampDataLoader tileStampDataLoader;
        private string currentPaletteRef = null;
        private TileSetModel model;

        #endregion Private Fields

        #region Public Constructors

        public TileStampEditorVM(
            ILogger logger,
            TileAtlasDataProvider tileSetsDataProvider,
            PalettesDataProvider palettesDataProvider,
            ITileStampDataLoader tileStampDataLoader,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IDrawingFactory drawingFactory,
            IBitmapProvider bitmapProvider,
            RenderViewBaseVM renderViewVm) : base(logger, workspaceMan, dialogProvider)
        {
            PaletteIds = new ObservableCollection<string>();
            this.tileSetsDataProvider = tileSetsDataProvider;
            this.palettesDataProvider = palettesDataProvider;
            this.tileStampDataLoader = tileStampDataLoader;
            View = renderViewVm;
        }

        #endregion Public Constructors

        #region Public Properties

        public RenderViewBaseVM View { get; }

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

            var tileStamp = tileStampDataLoader.LoadObject(entry.Id);


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

        private void SwitchPalette()
        {
            CurrentPalette = palettesDataProvider.GetPalette(CurrentPaletteRef);
        }

        #endregion Private Methods
    }
}
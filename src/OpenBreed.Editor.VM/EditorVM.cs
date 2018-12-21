﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Levels;
using OpenBreed.Editor.VM.Tiles;
using System.Drawing;
using OpenBreed.Common.Maps.Readers.MAP;
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Levels.Tools;
using OpenBreed.Common.Palettes;
using System.Diagnostics;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Editor.Cfg;
using System.ComponentModel;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Sources;
using OpenBreed.Common.Database.Items.Props;
using OpenBreed.Common.Database.Items.Levels;
using OpenBreed.Common.Database.Items.Tiles;
using OpenBreed.Common.Database.Items.Sprites;
using OpenBreed.Common.Database.Items.Palettes;

namespace OpenBreed.Editor.VM
{
    public enum EditorState
    {
        Active,
        Exiting,
        Exited
    }

    public class EditorVM : BaseViewModel, IDisposable
    {

        #region Private Fields

        private PropSetVM _propSet;
        private DatabaseVM _database;
        private EditorState _state;

        #endregion Private Fields

        #region Public Constructors

        public EditorVM(IDialogProvider dialogProvider)
        {
            DialogProvider = dialogProvider;

            Settings = new SettingsMan();
            ToolsMan = new ToolsMan();
            TileSetSelector = new TileSetSelectorVM(this);

            SpriteSets = new BindingList<SpriteSetVM>();
            SpriteSets.ListChanged += (s, e) => OnPropertyChanged(nameof(SpriteSets));

            TileSets = new BindingList<TileSetVM>();
            TileSets.ListChanged += (s, e) => OnPropertyChanged(nameof(TileSets));

            Palettes = new BindingList<PaletteVM>();
            Palettes.ListChanged += (s, e) => OnPropertyChanged(nameof(Palettes));

            LevelPropSelector = new LevelPropSelectorVM(this);
            PropSetEditor = new PropSetEditorVM(this);
            DatabaseViewer = new DatabaseViewerVM(this);
            TileSetViewer = new TileSetViewerVM(this);
            SpriteSetViewer = new SpriteSetSelectorVM(this);
            SpriteViewer = new SpriteViewerVM(this);
            PaletteViewer = new PalettesVM(this);
            ImageViewer = new ImageViewerVM(this);
            LevelEditor = new LevelEditorVM(this);
            Level = new LevelVM(this);
            MapBodyViewer = new LevelBodyEditorVM(this);
            FormatMan = new DataFormatMan();
            SourceMan = new SourceMan();
            SourceMan.ExpandVariables = Settings.ExpandVariables;

            FormatMan.RegisterFormat("ABSE_MAP", new ABSEMAPFormat());
            FormatMan.RegisterFormat("ABHC_MAP", new ABHCMAPFormat());
            FormatMan.RegisterFormat("ABTA_MAP", new ABTAMAPFormat());
            FormatMan.RegisterFormat("ABTABLK", new ABTABLKFormat());
            FormatMan.RegisterFormat("ABTASPR", new ABTASPRFormat());
            FormatMan.RegisterFormat("ACBM_TILE_SET", new ACBMTileSetFormat());
            FormatMan.RegisterFormat("ACBM_IMAGE", new ACBMImageFormat());
            FormatMan.RegisterFormat("PALETTE", new PaletteFormat());
        }

        #endregion Public Constructors

        #region Public Properties

        public DatabaseVM Database
        {
            get { return _database; }
            set { SetProperty(ref _database, value); }
        }

        public LevelPropSelectorVM LevelPropSelector { get; }
        public PropSetEditorVM PropSetEditor { get; }

        public DatabaseViewerVM DatabaseViewer { get; }

        public IDialogProvider DialogProvider { get; }

        public ImageViewerVM ImageViewer { get; }

        public LevelEditorVM LevelEditor { get; }

        public LevelVM Level { get; private set; }

        public LevelBodyEditorVM MapBodyViewer { get; private set; }

        public PalettesVM PaletteViewer { get; private set; }

        public PropSetVM PropSet
        {
            get { return _propSet; }
            set { SetProperty(ref _propSet, value); }
        }

        public SettingsMan Settings { get; private set; }

        public DataFormatMan FormatMan { get; }
        public SourceMan SourceMan { get; }

        public BindingList<SpriteSetVM> SpriteSets { get; private set; }

        public BindingList<PaletteVM> Palettes { get; private set; }

        public SpriteSetSelectorVM SpriteSetViewer { get; set; }

        public SpriteViewerVM SpriteViewer { get; set; }

        public EditorState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public BindingList<TileSetVM> TileSets { get; private set; }

        public TileSetSelectorVM TileSetSelector { get; private set; }

        public TileSetViewerVM TileSetViewer { get; private set; }

        public ToolsMan ToolsMan { get; private set; }

        #endregion Public Properties

        #region Public Methods


        public void LoadLevel(LevelDef levelDef)
        {
            TileSets.Clear();
            SpriteSets.Clear();
            Palettes.Clear();

            if (levelDef.TileSetRef != null)
            {
                var tileSetDef = Database.GetTileSetDef(levelDef.TileSetRef);
                if (tileSetDef == null)
                    throw new Exception($"No Tile set definition with name '{levelDef.TileSetRef}' found!");

                AddTileSet(tileSetDef);
                TileSetSelector.CurrentItem = TileSets.FirstOrDefault();
            }


            foreach (var spriteSetRef in levelDef.SpriteSetRefs)
            {
                var spriteSetDef = Database.GetSpriteSetDef(spriteSetRef);
                if (spriteSetDef == null)
                    throw new Exception($"No Sprite set definition with name '{spriteSetRef}' found!");

                AddSpriteSet(spriteSetDef);
            }

            SpriteSetViewer.CurrentItem = SpriteSets.FirstOrDefault();

            if (SpriteSetViewer.CurrentItem != null)
                SpriteViewer.CurrentItem = SpriteSetViewer.CurrentItem.Items.FirstOrDefault();

            if (levelDef.PropertySetRef != null)
            {
                var propSetDef = Database.GetPropertySetDef(levelDef.PropertySetRef);
                if (propSetDef == null)
                    throw new Exception($"No Prop set definition with name '{levelDef.PropertySetRef}' found!");

                LoadPropSet(propSetDef);
            }

            foreach (var paletteRef in levelDef.PaletteRefs)
            {
                var paletteDef = Database.GetPaletteDef(paletteRef);
                if (paletteDef == null)
                    throw new Exception($"No Palette definition with name '{paletteRef}' found!");

                AddPalette(paletteDef);

                PaletteViewer.CurrentItem = PaletteViewer.Items.FirstOrDefault();
            }

            var map = CreateMap();
            map.Load(levelDef);
            Level = map;
        }

        public void LoadPropSet(PropertySetDef propSetDef)
        {
            var propSet = CreatePropSet();
            propSet.Load(propSetDef);
            PropSet = propSet;
        }

        public void AddPalette(PaletteDef paletteDef)
        {
            var newPalette = CreatePalette();
            newPalette.Load(paletteDef);
            Palettes.Add(newPalette);
        }

        public void AddSpriteSet(SpriteSetDef spriteSetDef)
        {
            var newSpriteSet = CreateSpriteSet();
            newSpriteSet.Load(spriteSetDef);
            SpriteSets.Add(newSpriteSet);
        }

        public void AddTileSet(TileSetDef tileSetDef)
        {
            var newTileSet = CreateTileSet();
            newTileSet.Load(tileSetDef);
            TileSets.Add(newTileSet);
        }

        public LevelVM CreateMap()
        {
            return new LevelVM(this);
        }

        public SpriteSetVM CreateSpriteSet()
        {
            return new SpriteSetVM(this);
        }

        public TileSetVM CreateTileSet()
        {
            return new TileSetVM(this);
        }

        public PaletteVM CreatePalette()
        {
            return new PaletteVM(this);
        }

        public PropSetVM CreatePropSet()
        {
            return new PropSetVM(this);
        }

        public DatabaseVM CreateDatabase()
        {
            return new DatabaseVM(this);
        }

        public void Dispose()
        {
            Settings.Store();
        }

        public void Initialize()
        {
            LevelPropSelector.Connect();
            PropSetEditor.Connect();
            DatabaseViewer.Connect();
        }
        public void Run()
        {
            try
            {
                Settings.Restore();
            }
            catch (Exception ex)
            {
                DialogProvider.ShowMessage("Critical exception: " + ex, "Critial exception");
            }
        }

        public void SaveDatabase(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool TryCloseDatabase()
        {
            return EditorVMHelper.TryCloseDatabase(this);
        }

        public bool TryExit()
        {
            return EditorVMHelper.TryExit(this);
        }

        internal bool TrySaveDatabase()
        {
            return EditorVMHelper.TrySaveDatabase(this);
        }

        public void TryOpenDatabase()
        {
            EditorVMHelper.TryOpenDatabase(this);
        }
        public void TryRunABTAGame()
        {
            Tools.TryAction(RunABTAGame);
        }

        #endregion Public Methods

        #region Private Methods

        private void RunABTAGame()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Settings.Cfg.Options.ABTA.GameRunFilePath;
            proc.StartInfo.Arguments = Settings.Cfg.Options.ABTA.GameRunFileArgs;
            proc.StartInfo.WorkingDirectory = Settings.Cfg.Options.ABTA.GameFolderPath;
            proc.Start();
        }

        #endregion Private Methods

    }
}

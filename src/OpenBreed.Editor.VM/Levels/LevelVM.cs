using OpenBreed.Common.Commands;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Common.Assets;
using System;
using System.IO;
using System.Linq;
using System.ComponentModel;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Props;
using System.Collections.Generic;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Common.Tiles;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Props;
using OpenBreed.Common;

namespace OpenBreed.Editor.VM.Levels
{
    public class LevelVM : EditableEntryVM
    {

        #region Private Fields

        private readonly CommandMan m_Commands;

        private bool _isModified;
        private PropSetVM _propSet;
        private AssetBase _source = null;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public LevelVM()
        {
            Body = new LevelBodyVM(this);
            Properties = new LevelPropertiesVM(this);

            Palettes = new BindingList<PaletteVM>();
            Palettes.ListChanged += (s, e) => OnPropertyChanged(nameof(Palettes));

            TileSets = new BindingList<TileSetVM>();
            TileSets.ListChanged += (s, e) => OnPropertyChanged(nameof(TileSets));

            SpriteSets = new BindingList<SpriteSetVM>();
            SpriteSets.ListChanged += (s, e) => OnPropertyChanged(nameof(SpriteSets));

            m_Commands = new CommandMan();

            PropertyChanged += MapVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public LevelBodyVM Body { get; }

        public CommandMan Commands { get { return m_Commands; } }

        public bool IsModified
        {
            get { return _isModified; }
            internal set { SetProperty(ref _isModified, value); }
        }

        public bool IsOpened { get { return Source != null; } }

        public BindingList<PaletteVM> Palettes { get; }
        public LevelPropertiesVM Properties { get; }

        public PropSetVM PropSet
        {
            get { return _propSet; }
            set { SetProperty(ref _propSet, value); }
        }

        public AssetBase Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }
        public BindingList<SpriteSetVM> SpriteSets { get; }
        public BindingList<TileSetVM> TileSets { get; }
        public int TileSize { get { return 16; } }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Public Methods

        //public void AddSpriteSet(SpriteSetModel spriteSet)
        //{
        //    SpriteSets.Add(Root.CreateSpiteSet(spriteSet));
        //}

        //public void AddSpriteSet(string name)
        //{
        //    var newSpriteSet = Root.CreateSpriteSet();
        //    newSpriteSet.Load(name);
        //    SpriteSets.Add(newSpriteSet);
        //}

        //public void AddTileSet(TileSetModel tileSet)
        //{
        //    TileSets.Add(Root.CreateTileSet(tileSet));
        //}

        //public void LoadPropSet(IPropSetEntry propSet)
        //{
        //    PropSet = Root.CreatePropSet(propSet);
        //}

        public void Restore(List<PaletteModel> palettes)
        {
            Palettes.RaiseListChangedEvents = false;
            Palettes.Clear();

            foreach (var palette in palettes)
            {
                var paletteVM = new PaletteVM();
                paletteVM.Restore(palette);
                Palettes.Add(paletteVM);
            }

            Palettes.RaiseListChangedEvents = true;
            Palettes.ResetBindings();
        }

        public void Save()
        {
            //OnSaving(new EventArgs());
            //Source.Save(CurrentMap);
        }

        public void SaveAs(AssetBase newSource)
        {
            //OnSaving(new EventArgs());
            //Source = newSource;
            //Source.Save(CurrentMap);
        }

        public void TryClose()
        {
            Close();
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Close()
        {
            if (Source == null)
                throw new InvalidOperationException("There is not map loaded.");

            Source.Dispose();
            Source = null;
        }
        internal void ConnectEvents()
        {
            Body.ConnectEvents();
        }
        //internal void Load(string name)
        //{
        //    var model = ServiceLocator.Instance.GetService<DataProvider>().GetLevel(name);

        //    foreach (var spriteSet in model.SpriteSets)
        //        AddSpriteSet(spriteSet);

        //    foreach (var tileSet in model.TileSets)
        //        AddTileSet(tileSet);

        //    if(model.PropSet != null)
        //        LoadPropSet(model.PropSet);

        //    Properties.Load(model.Map);
        //    Body.Load(model.Map);

        //    Root.LevelEditor.PaletteSelector.PropertyChanged += PaletteSelector_PropertyChanged;

        //    Restore(model.Palettes);
        //    Root.LevelEditor.PaletteSelector.CurrentItem = Palettes.FirstOrDefault();
        //    Root.LevelEditor.BodyEditor.CurrentMapBody = Body;
        //}

        #endregion Internal Methods

        #region Private Methods

        private void MapVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Source):
                    UpdateTitle();
                    break;
                default:
                    break;
            }
        }

        //    Root.LevelEditor.BodyEditor.CurrentMapBody = Body;
        //}
        private string MarkNameIfModified()
        {
            if (IsModified)
                return Source.Name + "*";
            else
                return Source.Name;
        }

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


        //internal void Load(LevelDef levelDef)
        //{
        //    var asset = Root.DataProvider.AssetsProvider.GetAsset(levelDef.AssetRef);
        //    if (asset == null)
        //        throw new Exception("Level source error: " + levelDef.AssetRef);

        //    var model = Root.DataProvider.FormatMan.Load(asset, levelDef.Format) as MapModel;
        //    Source = asset;

        //    if (levelDef.TileSetRef != null)
        //        AddTileSet(levelDef.TileSetRef);

        //    if (levelDef.PropertySetRef != null)
        //        LoadPropSet(levelDef.PropertySetRef);

        //    foreach (var spriteSetRef in levelDef.SpriteSetRefs)
        //        AddSpriteSet(spriteSetRef);

        //    foreach (var paletteRef in levelDef.PaletteRefs)
        //        AddPalette(paletteRef);

        //    Properties.Load(model);
        //    Body.Load(model);

        //    if (!levelDef.PaletteRefs.Any())
        //    {
        //        Restore(model.Properties.Palettes);
        //        Root.LevelEditor.PaletteSelector.CurrentItem = Palettes.FirstOrDefault();
        //    }
        private void UpdateTitle()
        {
            if (Source == null)
                Title = "<No map>";
            else
                Title = MarkNameIfModified();
        }

        #endregion Private Methods

    }
}
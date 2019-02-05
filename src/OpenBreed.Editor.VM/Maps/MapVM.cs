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

namespace OpenBreed.Editor.VM.Maps
{
    public class MapVM : EditableEntryVM
    {

        #region Private Fields

        private readonly CommandMan m_Commands;

        private bool _isModified;
        private PropSetVM _propSet;
        private AssetBase _source = null;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public MapVM()
        {
            TileSets = new BindingList<TileSetVM>();
            Layout = new MapLayoutVM(this);
            Properties = new LevelPropertiesVM(this);

            Palettes = new BindingList<PaletteVM>();
            Palettes.ListChanged += (s, e) => OnPropertyChanged(nameof(Palettes));

            SpriteSets = new BindingList<SpriteSetVM>();
            SpriteSets.ListChanged += (s, e) => OnPropertyChanged(nameof(SpriteSets));

            m_Commands = new CommandMan();

            PropertyChanged += MapVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public CommandMan Commands { get { return m_Commands; } }

        public bool IsModified
        {
            get { return _isModified; }
            internal set { SetProperty(ref _isModified, value); }
        }

        public bool IsOpened { get { return Source != null; } }
        public MapLayoutVM Layout { get; }


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

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);

            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.GetMap(entry.Id);

            SetTileSets(model.TileSets);

            //foreach (var spriteSet in model.SpriteSets)
            //    AddSpriteSet(spriteSet);

            if (model.PropSet != null)
                SetPropSet(model.PropSet);

            Properties.Load(model);

            Layout.FromModel(model.Layout);

            SetPalettes(model.Palettes);
        }

        #endregion Public Properties

        #region Public Methods

        public void SetTileSets(List<TileSetModel> tileSets)
        {
            TileSets.UpdateAfter(() =>
            {
                TileSets.Clear();

                foreach (var tileSet in tileSets)
                {
                    var tileSetVM = new TileSetVM();
                    tileSetVM.FromModel(tileSet);
                    TileSets.Add(tileSetVM);
                }
            });
        }

        public void SetPalettes(List<PaletteModel> palettes)
        {
            Palettes.UpdateAfter(() =>
            {
                Palettes.Clear();

                foreach (var palette in palettes)
                {
                    var paletteVM = new PaletteVM();
                    paletteVM.FromModel(palette);
                    Palettes.Add(paletteVM);
                }
            });
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
            Layout.ConnectEvents();
        }

        internal void SetPropSet(IPropSetEntry propSet)
        {
            var propSetVM = new PropSetVM();

            propSetVM.FromModel(propSet);

            PropSet = propSetVM;
        }

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
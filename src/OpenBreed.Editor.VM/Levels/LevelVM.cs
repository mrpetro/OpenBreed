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
using OpenBreed.Common.Commands;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Sources;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Project;
using OpenBreed.Editor.VM.Sources;
using System;
using System.Linq;

namespace OpenBreed.Editor.VM.Maps
{
    public delegate void CurrentMapChangedEventHandler(object sender, CurrentMapChangedEventArgs e);

    public delegate void CurrentPaletteChangedEventHandler(object sender, CurrentPaletteChangedEventArgs e);

    public delegate void MapModifiedEventHandler(object sender, EventArgs e);

    public delegate void MapSavingEventHandler(object sender, EventArgs e);

    public class CurrentMapChangedEventArgs : EventArgs
    {

        #region Public Constructors

        public CurrentMapChangedEventArgs(MapModel map)
        {
            Map = map;
        }

        #endregion Public Constructors

        #region Public Properties

        public MapModel Map { get; set; }

        #endregion Public Properties

    }

    public class CurrentPaletteChangedEventArgs : EventArgs
    {

        #region Public Constructors

        public CurrentPaletteChangedEventArgs(PaletteModel palette)
        {
            Palette = palette;
        }

        #endregion Public Constructors

        #region Public Properties

        public PaletteModel Palette { get; set; }

        #endregion Public Properties

    }

    public class MapVM : BaseViewModel
    {

        #region Private Fields

        private readonly CommandMan m_Commands;

        private BaseSource _source = null;

        private string _title;

        private MapModel m_CurrentMap = null;
        private bool m_IsModified;

        #endregion Private Fields

        #region Public Constructors

        public MapVM(EditorVM root)
        {
            Root = root;
            Body = new MapBodyVM(this);
            Properties = new MapPropertiesVM(this);

            m_Commands = new CommandMan();

            PropertyChanged += MapVM_PropertyChanged;
        }

        public void Set(string mapSourceRef)
        {
            var mapSourceDef = Root.CurrentDatabase.GetSourceDef(mapSourceRef);
            if (mapSourceDef == null)
                throw new Exception("No MapSource definition found!");

            var source = Root.Sources.GetSource(mapSourceDef);

            if (source == null)
                throw new Exception("Map source error: " + mapSourceRef);

            Load(mapSourceDef);
        }

        #endregion Public Constructors

        #region Public Events

        public event CurrentMapChangedEventHandler CurrentMapChanged;

        public event MapModifiedEventHandler Modified;

        public event MapSavingEventHandler Saving;

        #endregion Public Events

        #region Public Properties

        public MapBodyVM Body { get; private set; }

        public CommandMan Commands { get { return m_Commands; } }

        public bool IsModified { get { return m_IsModified; } }

        //            OnCurrentMapChanged(new CurrentMapChangedEventArgs(m_CurrentMap));
        //        }
        //    }
        //}
        public bool IsModifiedInternal
        {
            set
            {
                if (m_IsModified != value)
                {
                    m_IsModified = value;
                    OnModified(new EventArgs());
                }
            }
        }

        //    set
        //    {
        //        if (m_CurrentMap != value)
        //        {
        //            m_CurrentMap = value;
        //            Update(CurrentMap);
        public bool IsOpened { get { return m_CurrentMap != null; } }

        public MapPropertiesVM Properties { get; private set; }

        //public MapModel CurrentMap
        //{
        //    get
        //    {
        //        return m_CurrentMap;
        //    }
        public EditorVM Root { get; private set; }

        public BaseSource Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }

        public int TileSize { get { return 16; } }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Save()
        {
            //OnSaving(new EventArgs());
            //Source.Save(CurrentMap);
        }

        public void SaveAs(BaseSource newSource)
        {
            //OnSaving(new EventArgs());
            //Source = newSource;
            //Source.Save(CurrentMap);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void ConnectEvents()
        {
            Body.ConnectEvents();
        }

        internal void Load(SourceDef sourceDef)
        {
            if (Source != null)
                throw new InvalidOperationException("Other map already loaded!");

            var source = Root.Sources.GetSource(sourceDef);

            var map = source.Load() as MapModel;
            Source = source;

            Properties.Load(map);
            Body.Load(map);

            Root.Palettes.Restore(map.Properties.Palettes);
            Root.Palettes.CurrentItem = Root.Palettes.Items.FirstOrDefault();
            Root.MapBodyViewer.CurrentMapBody = Body;
        }

        #endregion Internal Methods

        #region Protected Methods

        protected virtual void OnCurrentMapChanged(CurrentMapChangedEventArgs e)
        {
            if (CurrentMapChanged != null) CurrentMapChanged(this, e);
        }

        protected virtual void OnModified(EventArgs e)
        {
            if (Modified != null) Modified(this, e);
        }

        protected virtual void OnSaving(EventArgs e)
        {
            if (Saving != null) Saving(this, e);
        }

        #endregion Protected Methods

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
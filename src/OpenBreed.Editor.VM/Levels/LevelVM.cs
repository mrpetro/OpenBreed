using OpenBreed.Common.Commands;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Common.Sources;
using System;
using System.IO;
using System.Linq;
using OpenBreed.Common.Database.Items.Levels;

namespace OpenBreed.Editor.VM.Levels
{
    public class LevelVM : BaseViewModel
    {

        #region Private Fields

        private readonly CommandMan m_Commands;

        private bool _isModified;
        private BaseSource _source = null;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public LevelVM(EditorVM root)
        {
            Root = root;
            Body = new LevelBodyVM(this);
            Properties = new LevelPropertiesVM(this);

            m_Commands = new CommandMan();

            PropertyChanged += MapVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public LevelBodyVM Body { get; private set; }

        public CommandMan Commands { get { return m_Commands; } }

        public bool IsModified
        {
            get { return _isModified; }
            internal set { SetProperty(ref _isModified, value); }
        }

        public bool IsOpened { get { return Source != null; } }

        public LevelPropertiesVM Properties { get; }

        public EditorVM Root { get; }

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

        internal void Load(LevelDef levelDef)
        {
            var sourceDef = Root.Database.GetSourceDef(levelDef.SourceRef);
            if (sourceDef == null)
                throw new Exception("No Source definition found with name: " + levelDef.SourceRef);

            var source = Root.SourceMan.GetSource(sourceDef);
            if (source == null)
                throw new Exception("TileSet source error: " + sourceDef);

            var model = Root.FormatMan.Load(source, levelDef.Format) as MapModel;
            Source = source;

            Properties.Load(model);
            Body.Load(model);

            if (!levelDef.PaletteRefs.Any())
            {
                Root.PaletteViewer.Restore(model.Properties.Palettes);
                Root.PaletteViewer.CurrentItem = Root.PaletteViewer.Items.FirstOrDefault();
            }

            Root.MapBodyViewer.CurrentMapBody = Body;
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
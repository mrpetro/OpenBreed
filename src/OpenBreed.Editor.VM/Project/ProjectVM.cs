using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM.Levels;
using OpenBreed.Editor.VM.Levels.Readers.XML;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Sources;
using OpenBreed.Editor.VM.Database.Sources;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Maps.Tools;

namespace OpenBreed.Editor.VM.Project
{
    public delegate void CurrentProjectChangedEventHandler(object sender, CurrentProjectChangedEventArgs e);

    public class CurrentProjectChangedEventArgs : EventArgs
    {
        public LevelDef Project { get; set; }

        public CurrentProjectChangedEventArgs(LevelDef project)
        {
            Project = project;
        }
    }

    public class ProjectVM
    {
        private LevelDef m_CurrentProject = null;

        public EditorVM Root { get; private set; }

        public bool IsProjectOpened { get { return CurrentProject != null; } }

        public event CurrentProjectChangedEventHandler CurrentProjectChanged;

        public LevelDef CurrentProject
        {
            get
            {
                return m_CurrentProject;
            }

            set
            {
                if (m_CurrentProject != value)
                {
                    m_CurrentProject = value;
                    OnCurrentProjectChanged(new CurrentProjectChangedEventArgs(m_CurrentProject));
                }
            }
        }

        public ProjectVM(EditorVM root)
        {
            Root = root;
        }

        public void Load(SourceDef sourceDef)
        {
            var source = Root.Sources.GetSource(sourceDef);

            var projectDef = source.Load() as LevelDef;

            Root.TileSets.Items.Clear();
            Root.TileSets.AddTileSet(projectDef.TileSetResourceRef);

            Root.SpriteSets.Items.Clear();
            foreach (var spriteSetSourceRef in projectDef.SpriteSetResourceRefs)
                Root.SpriteSets.AddSpriteSet(spriteSetSourceRef);

            if (projectDef.PropertySetResourceRef != null)
                Root.PropSets.AddPropertySet(projectDef.PropertySetResourceRef);

            var mapSourceDef = Root.CurrentDatabase.GetSourceDef(projectDef.MapResourceRef);
            if (mapSourceDef != null)
                Root.Map.Load(mapSourceDef);


            CurrentProject = projectDef;
        }

        protected virtual void OnCurrentProjectChanged(CurrentProjectChangedEventArgs e)
        {
            if (CurrentProjectChanged != null) CurrentProjectChanged(this, e);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM.Database;
using System.Collections;
using OpenBreed.Editor.VM.Levels.Builders;
using OpenBreed.Editor.VM.Levels.Readers.XML;
using OpenBreed.Editor.VM.Sources;

namespace OpenBreed.Editor.VM.Levels
{
    public class ProjectsHandler
    {
        private readonly EditorVM m_Editor;

        public EditorVM Editor { get { return m_Editor; } }

        public ProjectsHandler(EditorVM editorModel)
        {
            m_Editor = editorModel;
        }

        public List<LevelDef> GetLevelDefList()
        {
            var sourceDefs = Editor.CurrentDatabase.GetAllSourcesOfType("LevelXML");

            List<LevelDef> levelDefs = new List<LevelDef>();

            foreach (var sourceDef in sourceDefs)
            {
                var source = Editor.Sources.GetSource(sourceDef);

                if (source == null)
                    continue;

                var levelDef = source.Load() as LevelDef;

                if(levelDef != null)
                    levelDefs.Add(levelDef);
            }

            return levelDefs;
        }

        public void Load(BaseSource source)
        {
        }

        public void Save()
        {

        }

        public ProjectDef GetModel(string levelRef)
        {
            var levelSourceDef = Editor.CurrentDatabase.GetSourceDef(levelRef);

            if (levelSourceDef == null)
                throw new Exception("No Level definition found with name: " + levelSourceDef);

            var source = Editor.Sources.GetSource(levelSourceDef);

            if (source == null)
                throw new Exception("Level source error: " + levelRef);

            return source.Load() as ProjectDef;
        }

        public ProjectDef GetModel(LevelDef levelDef)
        {
            var levelBuilder = LevelBuilder.NewLevel();

            levelBuilder.SetId(levelDef.Id);
            levelBuilder.SetName(levelDef.Name);

            Editor.Map.Set(levelDef.MapResourceRef);

            Editor.TileSets.Items.Clear();
            Editor.TileSets.AddTileSet(levelDef.TileSetResourceRef);

            Editor.PropSets.Items.Clear();
            Editor.PropSets.AddPropertySet(levelDef.PropertySetResourceRef);

            Editor.SpriteSets.Items.Clear();
            foreach (var spriteSetSourceRef in levelDef.SpriteSetResourceRefs)
                Editor.SpriteSets.AddSpriteSet(spriteSetSourceRef);

            return levelBuilder.Build();
        }
    }
}

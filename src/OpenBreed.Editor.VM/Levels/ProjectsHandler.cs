using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM.Database;
using System.Collections;
using OpenBreed.Editor.VM.Sources;
using OpenBreed.Editor.VM.Levels.Readers.XML;

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Common.Database.Sources;

namespace OpenBreed.Editor.VM
{
    public class GameDatabase
    {
        private readonly GameDatabaseDef m_DatabaseDef;
        private readonly string m_FilePath;

        public string FilePath { get { return m_FilePath; } }

        public List<LevelDef> LevelDefs { get { return m_DatabaseDef.LevelDefs; } }

        public GameDatabase(GameDatabaseDef databaseDef, string filePath)
        {
            m_DatabaseDef = databaseDef;
            m_FilePath = filePath;
        }

        public void SaveChanges()
        {
        }

        public List<SourceDef> GetAllSourcesOfType(string type)
        {
            return m_DatabaseDef.SourceDefs.FindAll(item => item.Type == type);
        }

        public SourceDef GetSourceDef(string sourceRef)
        {
            var sourceDef = m_DatabaseDef.SourceDefs.FirstOrDefault(item => item.Name == sourceRef);

            if (sourceDef == null)
                throw new InvalidOperationException("Source " + sourceRef + " not found!");

            return sourceDef;
        }

        public LevelDef GetLevelDef(int id)
        {
            var levelDef = m_DatabaseDef.LevelDefs.FirstOrDefault(item => item.Id == id);

            if (levelDef == null)
                throw new InvalidOperationException("Level(" + id + ") not found!");

            return levelDef;
        }

    }
}

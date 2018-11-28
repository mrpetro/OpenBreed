using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using OpenBreed.Editor.VM.Database.Sources;
using OpenBreed.Editor.VM.Database.Resources;
using OpenBreed.Editor.VM.Levels.Readers.XML;
using OpenBreed.Common.Logging;
using OpenBreed.Common;

namespace OpenBreed.Editor.VM.Database
{
    [Serializable]
    public class GameDatabaseDef
    {
        public const string DEFAULT_ABHC_DB_PATH = @"Resources\ABHC\GameDatabase.ABHC.xml";
        public const string DEFAULT_ABTA_DB_PATH = @"Resources\ABTA\GameDatabase.ABTA.EPF.xml";

        [XmlArray("SourceDefs"),
        XmlArrayItem("DirectoryFileSourceDef", typeof(DirectoryFileSourceDef)),
        XmlArrayItem("EPFArchiveFileSourceDef", typeof(EPFArchiveFileSourceDef))]
        public List<SourceDef> SourceDefs { get; set; }

        [XmlArray("ResourceDefs"),
        XmlArrayItem("ResourceLevelDef", typeof(ResourceLevelDef))]
        public List<ResourceDef> ResourceDefs { get; set; }

        public List<LevelDef> LevelDefs { get; set; }

        public static GameDatabaseDef LoadDefault()
        {
            try
            {
                string defaultPath = Path.Combine(ProgramTools.AppDir, DEFAULT_ABTA_DB_PATH);
                GameDatabaseDef databaseDef = Tools.RestoreFromXml<GameDatabaseDef>(defaultPath);
                LogMan.Instance.LogSuccess("Default Database loaded successful from " + defaultPath);
                return databaseDef;
            }
            catch (Exception ex)
            {
                LogMan.Instance.LogError("Unable to load default Database. Reason: " + ex.Message);
            }

            return null;
        }

        public static GameDatabaseDef Load(string filePath)
        {
            return Tools.RestoreFromXml<GameDatabaseDef>(filePath);
        }
    }
}

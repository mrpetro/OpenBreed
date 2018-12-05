using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using OpenBreed.Common.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Database.Resources;
using OpenBreed.Common.Database.Sources;

namespace OpenBreed.Common.Database.Sources
{
    [Serializable]
    public class GameDatabaseDef
    {
        public const string DEFAULT_DATABASE_DIR_NAME = "Defaults";

        [XmlArray("SourceDefs"),
        XmlArrayItem("DirectoryFileSourceDef", typeof(DirectoryFileSourceDef)),
        XmlArrayItem("EPFArchiveFileSourceDef", typeof(EPFArchiveFileSourceDef))]
        public List<SourceDef> SourceDefs { get; set; }

        [XmlArray("ResourceDefs"),
        XmlArrayItem("ResourceLevelDef", typeof(ResourceLevelDef))]
        public List<ResourceDef> ResourceDefs { get; set; }

        public List<LevelDef> LevelDefs { get; set; }

        public static string DefaultDirectoryPath
        {
            get { return Path.Combine(ProgramTools.AppDir, DEFAULT_DATABASE_DIR_NAME); }
        }

        public static GameDatabaseDef Load(string filePath)
        {
            return Tools.RestoreFromXml<GameDatabaseDef>(filePath);
        }
    }
}

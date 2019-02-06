using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase.Resources;
using OpenBreed.Common.XmlDatabase.Tables.Sources;
using OpenBreed.Common.XmlDatabase.Tables;
using OpenBreed.Common.XmlDatabase.Items.Images;
using OpenBreed.Common.XmlDatabase.Items.Maps;
using OpenBreed.Common.XmlDatabase.Tables.Images;
using OpenBreed.Common.XmlDatabase.Tables.Maps;
using OpenBreed.Common.XmlDatabase.Tables.Actions;
using OpenBreed.Common.XmlDatabase.Tables.Tiles;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Palettes;

namespace OpenBreed.Common.XmlDatabase
{
    [Serializable]
    public class DatabaseDef
    {

        #region Public Fields

        public const string DEFAULT_DATABASE_DIR_NAME = "Defaults";

        [XmlArray("Tables"),
        XmlArrayItem("Assets", typeof(DatabaseAssetTableDef)),
        XmlArrayItem("Maps", typeof(DatabaseMapTableDef)),
        XmlArrayItem("Palettes", typeof(DatabasePaletteTableDef)),
        XmlArrayItem("ActionSets", typeof(DatabaseActionSetTableDef)),
        XmlArrayItem("TileSets", typeof(DatabaseTileSetTableDef)),
        XmlArrayItem("SpriteSets", typeof(DatabaseSpriteSetTableDef)),
        XmlArrayItem("Sounds", typeof(DatabaseSoundTableDef)),
        XmlArrayItem("Images", typeof(DatabaseImageTableDef))]
        public readonly List<DatabaseTableDef> Tables = new List<DatabaseTableDef>();

        #endregion Public Fields

        #region Public Properties

        public static string DefaultDirectoryPath
        {
            get { return Path.Combine(ProgramTools.AppDir, DEFAULT_DATABASE_DIR_NAME); }
        }

        #endregion Public Properties

        #region Public Methods

        public static DatabaseDef Load(string filePath)
        {
            return Other.RestoreFromXml<DatabaseDef>(filePath);
        }

        public void Save(string xmlFilePath)
        {
            Other.StoreAsXml<DatabaseDef>(xmlFilePath, this);
        }

        #endregion Public Methods

    }
}

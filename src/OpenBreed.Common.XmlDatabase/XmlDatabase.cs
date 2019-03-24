using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase.Resources;
using OpenBreed.Common.XmlDatabase.Tables.Assets;
using OpenBreed.Common.XmlDatabase.Tables;
using OpenBreed.Common.XmlDatabase.Items.Images;
using OpenBreed.Common.XmlDatabase.Items.Maps;
using OpenBreed.Common.XmlDatabase.Tables.Images;
using OpenBreed.Common.XmlDatabase.Tables.Maps;
using OpenBreed.Common.XmlDatabase.Tables.Actions;
using OpenBreed.Common.XmlDatabase.Tables.Tiles;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Palettes;
using OpenBreed.Common.XmlDatabase.Tables.Texts;

namespace OpenBreed.Common.XmlDatabase
{
    [Serializable]
    [XmlRoot("Database")]
    public class XmlDatabase
    {

        #region Public Fields

        public const string DEFAULT_DATABASE_DIR_NAME = "Defaults";

        [XmlArray("Tables"),
        XmlArrayItem("Assets", typeof(XmlDbAssetTableDef)),
        XmlArrayItem("Maps", typeof(XmlDbMapTableDef)),
        XmlArrayItem("Palettes", typeof(XmlDbPaletteTableDef)),
        XmlArrayItem("Texts", typeof(XmlDbTextTableDef)),
        XmlArrayItem("ActionSets", typeof(XmlDbActionSetTableDef)),
        XmlArrayItem("TileSets", typeof(XmlDbTileSetTableDef)),
        XmlArrayItem("SpriteSets", typeof(XmlDbSpriteSetTableDef)),
        XmlArrayItem("Sounds", typeof(DatabaseSoundTableDef)),
        XmlArrayItem("Images", typeof(XmlDbImageTableDef))]
        public readonly List<XmlDbTableDef> Tables = new List<XmlDbTableDef>();

        #endregion Public Fields

        #region Public Properties

        public static string DefaultDirectoryPath
        {
            get { return Path.Combine(ProgramTools.AppDir, DEFAULT_DATABASE_DIR_NAME); }
        }

        #endregion Public Properties

        #region Public Methods

        public static XmlDatabase Load(string filePath)
        {
            return Other.RestoreFromXml<XmlDatabase>(filePath);
        }

        public void Save(string xmlFilePath)
        {
            Other.StoreAsXml<XmlDatabase>(xmlFilePath, this);
        }

        #endregion Public Methods

    }
}

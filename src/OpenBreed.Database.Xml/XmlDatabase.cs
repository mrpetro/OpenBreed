using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using OpenBreed.Common;
using OpenBreed.Database.Xml.Resources;
using OpenBreed.Database.Xml.Tables;
using OpenBreed.Database.Xml.Items.Images;
using OpenBreed.Database.Xml.Items.Maps;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;

namespace OpenBreed.Database.Xml
{
    [Serializable]
    [XmlRoot("Database")]
    public class XmlDatabase
    {

        #region Public Fields

        public const string DEFAULT_DATABASE_DIR_NAME = "Defaults";

        [XmlArray("Tables"),
        XmlArrayItem(XmlDbDataSourceTableDef.NAME, typeof(XmlDbDataSourceTableDef)),
        XmlArrayItem(XmlDbMapTableDef.NAME, typeof(XmlDbMapTableDef)),
        XmlArrayItem(XmlDbPaletteTableDef.NAME, typeof(XmlDbPaletteTableDef)),
        XmlArrayItem(XmlDbTextTableDef.NAME, typeof(XmlDbTextTableDef)),
        XmlArrayItem(XmlDbActionSetTableDef.NAME, typeof(XmlDbActionSetTableDef)),
        XmlArrayItem("TileSets", typeof(XmlDbTileAtlasTableDef)),
        XmlArrayItem("TileStamps", typeof(XmlDbTileStampTableDef)),
        XmlArrayItem("SpriteSets", typeof(XmlDbSpriteAtlasTableDef)),
        XmlArrayItem(XmlDbSoundTableDef.NAME, typeof(XmlDbSoundTableDef)),
        XmlArrayItem(XmlDbSongTableDef.NAME, typeof(XmlDbSongTableDef)),
        XmlArrayItem(XmlDbImageTableDef.NAME, typeof(XmlDbImageTableDef)),
        XmlArrayItem(XmlDbScriptTableDef.NAME, typeof(XmlDbScriptTableDef)),
        XmlArrayItem(XmlDbAnimationTableDef.NAME, typeof(XmlDbAnimationTableDef)),
        XmlArrayItem(XmlDbEntityTemplateTableDef.NAME, typeof(XmlDbEntityTemplateTableDef)),
        XmlArrayItem(XmlDbEntityClassTableDef.NAME, typeof(XmlDbEntityClassTableDef))]
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
            return XmlHelper.RestoreFromXml<XmlDatabase>(filePath);
        }

        public static XmlDatabase Empty()
        {
            return new XmlDatabase();
        }

        public void Save(string xmlFilePath)
        {
            XmlHelper.StoreAsXml<XmlDatabase>(xmlFilePath, this);
        }

        #endregion Public Methods

    }
}

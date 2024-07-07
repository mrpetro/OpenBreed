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
        XmlArrayItem("DataSources", typeof(XmlDbDataSourceTableDef)),
        XmlArrayItem("Maps", typeof(XmlDbMapTableDef)),
        XmlArrayItem("Palettes", typeof(XmlDbPaletteTableDef)),
        XmlArrayItem("Texts", typeof(XmlDbTextTableDef)),
        XmlArrayItem("ActionSets", typeof(XmlDbActionSetTableDef)),
        XmlArrayItem("TileSets", typeof(XmlDbTileAtlasTableDef)),
        XmlArrayItem("TileStamps", typeof(XmlDbTileStampTableDef)),
        XmlArrayItem("SpriteSets", typeof(XmlDbSpriteAtlasTableDef)),
        XmlArrayItem("Sounds", typeof(XmlDbSoundTableDef)),
        XmlArrayItem("Songs", typeof(XmlDbSongTableDef)),
        XmlArrayItem("Images", typeof(XmlDbImageTableDef)),
        XmlArrayItem("Scripts", typeof(XmlDbScriptTableDef)),
        XmlArrayItem("Animations", typeof(XmlDbAnimationTableDef)),
        XmlArrayItem("EntityTemplates", typeof(XmlDbEntityTemplateTableDef))]
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

        public void Save(string xmlFilePath)
        {
            XmlHelper.StoreAsXml<XmlDatabase>(xmlFilePath, this);
        }

        #endregion Public Methods

    }
}

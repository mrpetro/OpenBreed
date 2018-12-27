﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using OpenBreed.Common.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Database.Resources;
using OpenBreed.Common.Database.Tables.Sources;
using OpenBreed.Common.Database.Tables;
using OpenBreed.Common.Database.Items.Images;
using OpenBreed.Common.Database.Items.Levels;
using OpenBreed.Common.Database.Tables.Images;
using OpenBreed.Common.Database.Tables.Levels;
using OpenBreed.Common.Database.Tables.Props;
using OpenBreed.Common.Database.Tables.Tiles;
using OpenBreed.Common.Database.Tables.Sprites;
using OpenBreed.Common.Database.Tables.Palettes;

namespace OpenBreed.Common.Database
{
    [Serializable]
    public class DatabaseDef
    {

        #region Public Fields

        public const string DEFAULT_DATABASE_DIR_NAME = "Defaults";

        [XmlArray("Tables"),
        XmlArrayItem("Sources", typeof(DatabaseSourceTableDef)),
        XmlArrayItem("Levels", typeof(DatabaseLevelTableDef)),
        XmlArrayItem("Palettes", typeof(DatabasePaletteTableDef)),
        XmlArrayItem("PropertySets", typeof(DatabasePropertySetTableDef)),
        XmlArrayItem("TileSets", typeof(DatabaseTileSetTableDef)),
        XmlArrayItem("SpriteSets", typeof(DatabaseSpriteSetTableDef)),
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
            return Tools.RestoreFromXml<DatabaseDef>(filePath);
        }

        public void Save(string xmlFilePath)
        {
            Tools.StoreAsXml<DatabaseDef>(xmlFilePath, this);
        }

        #endregion Public Methods

    }
}

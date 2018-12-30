using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Database.Tables.Images;
using OpenBreed.Common.Database.Tables.Levels;
using OpenBreed.Common.Database.Tables.Palettes;
using OpenBreed.Common.Database.Tables.Props;
using OpenBreed.Common.Database.Tables.Sources;
using OpenBreed.Common.Database.Tables.Sprites;
using OpenBreed.Common.Database.Tables.Tiles;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public enum DatabaseMode
    {
        Create,
        Read,
        Update
    }

    public class XmlDatabase
    {

        #region Public Constructors

        public XmlDatabase(string xmlFilePath, DatabaseMode mode)
        {
            XmlFilePath = xmlFilePath;
            Mode = mode;

            if (Mode == DatabaseMode.Create)
                Data = new DatabaseDef();
            else
                Data = DatabaseDef.Load(XmlFilePath);
        }

        #endregion Public Constructors

        #region Public Properties

        public DatabaseMode Mode { get; }

        public string XmlFilePath { get; }

        #endregion Public Properties

        #region Internal Properties

        internal DatabaseDef Data { get; }

        #endregion Internal Properties

        #region Public Methods

        public void Save()
        {
            Data.Save(XmlFilePath);
        }

        #endregion Public Methods

        #region Internal Methods

        internal DatabaseImageTableDef GetImageTable()
        {
            var table = Data.Tables.OfType<DatabaseImageTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabaseImageTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal DatabaseLevelTableDef GetLevelTable()
        {
            var table = Data.Tables.OfType<DatabaseLevelTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabaseLevelTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }
        internal DatabasePaletteTableDef GetPaletteTable()
        {
            var table = Data.Tables.OfType<DatabasePaletteTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabasePaletteTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal DatabasePropertySetTableDef GetPropSetTable()
        {
            var table = Data.Tables.OfType<DatabasePropertySetTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabasePropertySetTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal DatabaseSourceTableDef GetSourcesTable()
        {
            var table = Data.Tables.OfType<DatabaseSourceTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabaseSourceTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal DatabaseSpriteSetTableDef GetSpriteSetTable()
        {
            var table = Data.Tables.OfType<DatabaseSpriteSetTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabaseSpriteSetTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal DatabaseTileSetTableDef GetTileSetTable()
        {
            var table = Data.Tables.OfType<DatabaseTileSetTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabaseTileSetTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        #endregion Internal Methods

    }
}

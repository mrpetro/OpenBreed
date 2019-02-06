using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.XmlDatabase.Tables.Images;
using OpenBreed.Common.XmlDatabase.Tables.Maps;
using OpenBreed.Common.XmlDatabase.Tables.Palettes;
using OpenBreed.Common.XmlDatabase.Tables.Actions;
using OpenBreed.Common.XmlDatabase.Tables.Sources;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Tiles;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.XmlDatabase
{


    public class XmlDatabase : IDatabase
    {

        #region Public Constructors

        public static XmlDatabase Open(string xmlFilePath)
        {
            return new XmlDatabase(xmlFilePath, DatabaseMode.Read);
        }

        private XmlDatabase(string xmlFilePath, DatabaseMode mode)
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

        public string Name { get { return XmlFilePath; } }

        public string XmlFilePath { get; }

        #endregion Public Properties

        #region Internal Properties

        internal DatabaseDef Data { get; }

        #endregion Internal Properties

        #region Public Methods

        public IUnitOfWork CreateUnitOfWork()
        {
            return new XmlUnitOfWork(this);
        }
        public void Save()
        {
            Data.Save($"{XmlFilePath.Replace(".xml","_out.xml")}");
        }

        #endregion Public Methods

        #region Internal Methods

        internal DatabaseAssetTableDef GetAssetsTable()
        {
            var table = Data.Tables.OfType<DatabaseAssetTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabaseAssetTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

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

        internal DatabaseMapTableDef GetMapsTable()
        {
            var table = Data.Tables.OfType<DatabaseMapTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabaseMapTableDef();
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

        internal DatabaseActionSetTableDef GetActionSetTable()
        {
            var table = Data.Tables.OfType<DatabaseActionSetTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabaseActionSetTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal DatabaseSoundTableDef GetSoundTable()
        {
            var table = Data.Tables.OfType<DatabaseSoundTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabaseSoundTableDef();
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

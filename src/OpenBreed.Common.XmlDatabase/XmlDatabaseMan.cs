using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Assets;
using OpenBreed.Common.XmlDatabase.Tables;

using OpenBreed.Common.Formats;
using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.XmlDatabase
{


    public class XmlDatabaseMan : IDatabase
    {

        #region Private Constructors

        private XmlDatabaseMan(string xmlFilePath, DatabaseMode mode)
        {
            XmlFilePath = xmlFilePath;
            Mode = mode;

            if (Mode == DatabaseMode.Create)
                Data = new XmlDatabase();
            else
                Data = XmlDatabase.Load(XmlFilePath);
        }

        #endregion Private Constructors

        #region Public Properties

        public DatabaseMode Mode { get; }

        public string Name { get { return XmlFilePath; } }

        public string XmlFilePath { get; }

        #endregion Public Properties

        #region Internal Properties

        internal XmlDatabase Data { get; }

        #endregion Internal Properties

        #region Public Methods

        public static XmlDatabaseMan Open(string xmlFilePath)
        {
            return new XmlDatabaseMan(xmlFilePath, DatabaseMode.Read);
        }
        public IUnitOfWork CreateUnitOfWork()
        {
            return new XmlUnitOfWork(this);
        }

        public void Save()
        {
            Data.Save(XmlFilePath);
            //Data.Save($"{XmlFilePath.Replace(".xml","_out.xml")}");
        }

        #endregion Public Methods

        #region Internal Methods

        internal XmlDbActionSetTableDef GetActionSetTable()
        {
            var table = Data.Tables.OfType<XmlDbActionSetTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new XmlDbActionSetTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal XmlDbDataSourceTableDef GetDataSourcesTable()
        {
            var table = Data.Tables.OfType<XmlDbDataSourceTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new XmlDbDataSourceTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal XmlDbAssetTableDef GetAssetsTable()
        {
            var table = Data.Tables.OfType<XmlDbAssetTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new XmlDbAssetTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal XmlDbImageTableDef GetImageTable()
        {
            var table = Data.Tables.OfType<XmlDbImageTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new XmlDbImageTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal XmlDbMapTableDef GetMapsTable()
        {
            var table = Data.Tables.OfType<XmlDbMapTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new XmlDbMapTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal XmlDbTextTableDef GetTextTable()
        {
            var table = Data.Tables.OfType<XmlDbTextTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new XmlDbTextTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal XmlDbPaletteTableDef GetPaletteTable()
        {
            var table = Data.Tables.OfType<XmlDbPaletteTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new XmlDbPaletteTableDef();
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
        internal XmlDbSpriteSetTableDef GetSpriteSetTable()
        {
            var table = Data.Tables.OfType<XmlDbSpriteSetTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new XmlDbSpriteSetTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        internal XmlDbTileSetTableDef GetTileSetTable()
        {
            var table = Data.Tables.OfType<XmlDbTileSetTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new XmlDbTileSetTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }

        #endregion Internal Methods

    }
}

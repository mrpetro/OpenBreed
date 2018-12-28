using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Sources;
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

            FormatMan = new DataFormatMan();

            FormatMan.RegisterFormat("ABSE_MAP", new ABSEMAPFormat());
            FormatMan.RegisterFormat("ABHC_MAP", new ABHCMAPFormat());
            FormatMan.RegisterFormat("ABTA_MAP", new ABTAMAPFormat());
            FormatMan.RegisterFormat("ABTABLK", new ABTABLKFormat());
            FormatMan.RegisterFormat("ABTASPR", new ABTASPRFormat());
            FormatMan.RegisterFormat("ACBM_TILE_SET", new ACBMTileSetFormat());
            FormatMan.RegisterFormat("ACBM_IMAGE", new ACBMImageFormat());
            FormatMan.RegisterFormat("PALETTE", new PaletteFormat());
        }

        #endregion Public Constructors

        #region Public Properties

        public DataFormatMan FormatMan { get; }
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

        internal DatabasePaletteTableDef GePaletteTable()
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

using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Database.Tables.Sources;
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

        public DatabaseSourceTableDef GetSourcesTable()
        {
            var table = Data.Tables.OfType<DatabaseSourceTableDef>().FirstOrDefault();
            if (table == null)
            {
                table = new DatabaseSourceTableDef();
                Data.Tables.Add(table);
            }
            return table;
        }
        public void Save()
        {
            Data.Save(XmlFilePath);
        }

        #endregion Public Methods

    }
}

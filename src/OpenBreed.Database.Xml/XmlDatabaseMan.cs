using OpenBreed.Database.Xml;
using OpenBreed.Database.Xml.Items.Assets;
using OpenBreed.Database.Xml.Tables;

using OpenBreed.Common.Formats;
using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Database.Interface;

namespace OpenBreed.Database.Xml
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



        public static XmlDatabaseMan Open(string xmlFilePath, bool readOnly = false)
        {
            var mode = DatabaseMode.Update;
            if (readOnly)
                mode = DatabaseMode.ReadOnly;

            return new XmlDatabaseMan(xmlFilePath, mode);
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new XmlUnitOfWork(this);
        }

        public void Save()
        {
            if (Mode == DatabaseMode.Update)
                Data.Save(XmlFilePath);
            else
                throw new InvalidOperationException("Database opened in 'ReadOnly' mode.");
        }

        #endregion Public Methods

        #region Internal Methods

        internal T GetTable<T>() where T : XmlDbTableDef, new()
        {
            var table = Data.Tables.OfType<T>().FirstOrDefault();
            if (table == null)
            {
                table = new T();
                Data.Tables.Add(table);
            }
            return table;
        }

        #endregion Internal Methods

    }
}

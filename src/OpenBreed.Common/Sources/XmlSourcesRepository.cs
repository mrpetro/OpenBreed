using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Formats;
using EPF;
using OpenBreed.Common.Logging;
using System.ComponentModel;
using System.Globalization;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Database.Tables.Sources;

namespace OpenBreed.Common.Sources
{
    public delegate string ExpandVariablesDelegate(string text);

    public class XmlSourcesRepository : IRepository<ISourceEntity>
    {

        #region Private Fields

        private readonly DatabaseSourceTableDef _table;
        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlSourcesRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetSourcesTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(ISourceEntity entity)
        {
            throw new NotImplementedException();
        }

        public ISourceEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ISourceEntity GetByName(string name)
        {
            var sourceDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (sourceDef == null)
                throw new Exception("No Source definition found with name: " + name);

            return sourceDef;
        }

        public void Remove(ISourceEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ISourceEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}

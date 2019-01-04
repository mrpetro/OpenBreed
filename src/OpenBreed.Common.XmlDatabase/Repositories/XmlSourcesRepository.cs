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
using OpenBreed.Common.Sources;
using OpenBreed.Common.XmlDatabase.Tables.Sources;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlSourcesRepository : IRepository<ISourceEntry>
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

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(ISourceEntry entity)
        {
            throw new NotImplementedException();
        }

        public ISourceEntry GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ISourceEntry GetByName(string name)
        {
            var sourceDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (sourceDef == null)
                throw new Exception("No Source definition found with name: " + name);

            return sourceDef;
        }

        public ISourceEntry GetNextTo(ISourceEntry entry)
        {
            throw new NotImplementedException();
        }

        public ISourceEntry GetPrevTo(ISourceEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Remove(ISourceEntry entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ISourceEntry entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}

using OpenBreed.Common.Props;
using OpenBreed.Common.XmlDatabase.Items.Props;
using OpenBreed.Common.XmlDatabase.Tables.Props;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlPropSetsRepository : IRepository<IPropSetEntry>
    {

        #region Private Fields

        private readonly DatabasePropertySetTableDef _table;

        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlPropSetsRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetPropSetTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get { return "Prop sets"; } }

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(IPropSetEntry entity)
        {
            throw new NotImplementedException();
        }

        public IPropSetEntry GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string name)
        {
            return _table.Items.FirstOrDefault(item => item.Name == name);
        }

        public IPropSetEntry GetByName(string name)
        {
            var propSetDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (propSetDef == null)
                throw new Exception("No Source definition found with name: " + name);

            return propSetDef;
        }

        public IPropSetEntry GetNextTo(IPropSetEntry entry)
        {
            var index = _table.Items.IndexOf((PropertySetDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public IPropSetEntry GetPreviousTo(IPropSetEntry entry)
        {
            var index = _table.Items.IndexOf((PropertySetDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }
        public void Remove(IPropSetEntry entity)
        {
            throw new NotImplementedException();
        }

        public void Update(IPropSetEntry entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}

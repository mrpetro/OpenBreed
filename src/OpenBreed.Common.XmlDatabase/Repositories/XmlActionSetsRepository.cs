using OpenBreed.Common.Actions;
using OpenBreed.Common.XmlDatabase.Items.Actions;
using OpenBreed.Common.XmlDatabase.Tables.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlActionSetsRepository : IRepository<IActionSetEntry>
    {

        #region Private Fields

        private readonly DatabaseActionSetTableDef _table;

        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlActionSetsRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetActionSetTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get { return "Action sets"; } }

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(IActionSetEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public IActionSetEntry GetById(string id)
        {
            var propSetDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (propSetDef == null)
                throw new Exception("No Source definition found with Id: " + id);

            return propSetDef;
        }

        public IActionSetEntry GetNextTo(IActionSetEntry entry)
        {
            var index = _table.Items.IndexOf((ActionSetDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public IActionSetEntry GetPreviousTo(IActionSetEntry entry)
        {
            var index = _table.Items.IndexOf((ActionSetDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }
        public void Remove(IActionSetEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IActionSetEntry entry)
        {
            var index = _table.Items.IndexOf((ActionSetDef)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (ActionSetDef)entry;
        }

        #endregion Public Methods

    }
}

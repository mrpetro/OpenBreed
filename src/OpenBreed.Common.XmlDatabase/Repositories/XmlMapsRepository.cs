using OpenBreed.Common.Maps;
using OpenBreed.Common.XmlDatabase.Items.Maps;
using OpenBreed.Common.XmlDatabase.Tables.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlMapsRepository : IRepository<IMapEntry>
    {

        #region Private Fields

        private readonly DatabaseMapTableDef _table;

        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlMapsRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetMapsTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get { return "Maps"; } }

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(IMapEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public IMapEntry GetById(string id)
        {
            var levelDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (levelDef == null)
                throw new Exception("No Level definition found with Id: " + id);

            return levelDef;
        }

        public IMapEntry GetNextTo(IMapEntry entry)
        {
            var index = _table.Items.IndexOf((MapDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public IMapEntry GetPreviousTo(IMapEntry entry)
        {
            var index = _table.Items.IndexOf((MapDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }
        public void Remove(IMapEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IMapEntry entry)
        {
            var index = _table.Items.IndexOf((MapDef)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (MapDef)entry;
        }

        #endregion Public Methods

    }
}

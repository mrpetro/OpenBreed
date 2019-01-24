using OpenBreed.Common.Maps;
using OpenBreed.Common.XmlDatabase.Items.Levels;
using OpenBreed.Common.XmlDatabase.Tables.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlLevelsRepository : IRepository<ILevelEntry>
    {

        #region Private Fields

        private readonly DatabaseLevelTableDef _table;

        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlLevelsRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetLevelTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get { return "Levels"; } }

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(ILevelEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public ILevelEntry GetById(string id)
        {
            var levelDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (levelDef == null)
                throw new Exception("No Level definition found with Id: " + id);

            return levelDef;
        }

        public ILevelEntry GetNextTo(ILevelEntry entry)
        {
            var index = _table.Items.IndexOf((LevelDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public ILevelEntry GetPreviousTo(ILevelEntry entry)
        {
            var index = _table.Items.IndexOf((LevelDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }
        public void Remove(ILevelEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(ILevelEntry entry)
        {
            var index = _table.Items.IndexOf((LevelDef)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (LevelDef)entry;
        }

        #endregion Public Methods

    }
}

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

        public ILevelEntry GetNextTo(ILevelEntry entry)
        {
            var index = _table.Items.IndexOf((LevelDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public ILevelEntry GetPrevTo(ILevelEntry entry)
        {
            var index = _table.Items.IndexOf((LevelDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }

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

        public IUnitOfWork UnitOfWork { get; }

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }

        #endregion Public Properties

        #region Public Methods

        public void Add(ILevelEntry entity)
        {
            throw new NotImplementedException();
        }

        public ILevelEntry GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ILevelEntry GetByName(string name)
        {
            var levelDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (levelDef == null)
                throw new Exception("No Level definition found with name: " + name);

            return levelDef;
        }

        public void Remove(ILevelEntry entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ILevelEntry entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}

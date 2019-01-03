using OpenBreed.Common.Maps;
using OpenBreed.Common.Sources;
using OpenBreed.Common.XmlDatabase.Items.Levels;
using OpenBreed.Common.XmlDatabase.Tables.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlLevelsRepository : IRepository<ILevelEntity>
    {

        public ILevelEntity GetNextTo(ILevelEntity entry)
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

        public ILevelEntity GetPrevTo(ILevelEntity entry)
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

        public IEnumerable<IEntity> Entries { get { return _table.Items; } }

        #endregion Public Properties

        #region Public Methods

        public void Add(ILevelEntity entity)
        {
            throw new NotImplementedException();
        }

        public ILevelEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ILevelEntity GetByName(string name)
        {
            var levelDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (levelDef == null)
                throw new Exception("No Level definition found with name: " + name);

            return levelDef;
        }

        public void Remove(ILevelEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ILevelEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}

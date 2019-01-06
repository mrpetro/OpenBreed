using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Items.Sprites;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlSpriteSetsRepository : IRepository<ISpriteSetEntry>
    {
        #region Private Fields

        private readonly DatabaseSpriteSetTableDef _table;

        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlSpriteSetsRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetSpriteSetTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(ISpriteSetEntry entity)
        {
            throw new NotImplementedException();
        }

        public ISpriteSetEntry GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ISpriteSetEntry GetByName(string name)
        {
            var spriteSetDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (spriteSetDef == null)
                throw new Exception("No Source definition found with name: " + name);

            return spriteSetDef;
        }

        public ISpriteSetEntry GetNextTo(ISpriteSetEntry entry)
        {
            var index = _table.Items.IndexOf((SpriteSetDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public ISpriteSetEntry GetPrevTo(ISpriteSetEntry entry)
        {
            var index = _table.Items.IndexOf((SpriteSetDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }

        public void Remove(ISpriteSetEntry entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ISpriteSetEntry entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}

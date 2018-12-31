using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlSpriteSetsRepository : IRepository<ISpriteSetEntity>
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

        public IEnumerable<IEntity> Entries { get { return _table.Items; } }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(ISpriteSetEntity entity)
        {
            throw new NotImplementedException();
        }

        public ISpriteSetEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ISpriteSetEntity GetByName(string name)
        {
            var spriteSetDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (spriteSetDef == null)
                throw new Exception("No Source definition found with name: " + name);

            return spriteSetDef;
        }

        public void Remove(ISpriteSetEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ISpriteSetEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}

using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Images;
using OpenBreed.Common.XmlDatabase.Tables.Images;
using OpenBreed.Common.Sounds;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlSoundsRepository : IRepository<ISoundEntity>
    {
        #region Private Fields

        private readonly DatabaseSoundTableDef _table;
        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlSoundsRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetSoundTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntity> Entries { get { return _table.Items; } }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(ISoundEntity entity)
        {
            throw new NotImplementedException();
        }

        public ISoundEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ISoundEntity GetByName(string name)
        {
            var entry = _table.Items.FirstOrDefault(item => item.Name == name);
            if (entry == null)
                throw new Exception("No Sound entry found with name: " + name);

            return entry;
        }

        public void Remove(ISoundEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ISoundEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}

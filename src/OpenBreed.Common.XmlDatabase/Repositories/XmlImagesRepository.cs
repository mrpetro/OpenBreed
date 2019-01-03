using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Images;
using OpenBreed.Common.XmlDatabase.Tables.Images;
using OpenBreed.Common.XmlDatabase.Items.Images;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlImagesRepository : IRepository<IImageEntity>
    {
        #region Private Fields

        private readonly DatabaseImageTableDef _table;

        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlImagesRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetImageTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntity> Entries { get { return _table.Items; } }

        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(IImageEntity entity)
        {
            throw new NotImplementedException();
        }

        public IImageEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IImageEntity GetByName(string name)
        {
            var spriteSetDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (spriteSetDef == null)
                throw new Exception("No Image definition found with name: " + name);

            return spriteSetDef;
        }

        public IImageEntity GetNextTo(IImageEntity entry)
        {
            var index = _table.Items.IndexOf((ImageDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public IImageEntity GetPrevTo(IImageEntity entry)
        {
            var index = _table.Items.IndexOf((ImageDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }
        public void Remove(IImageEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(IImageEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}

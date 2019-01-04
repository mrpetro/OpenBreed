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
    public class XmlImagesRepository : IRepository<IImageEntry>
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

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(IImageEntry entity)
        {
            throw new NotImplementedException();
        }

        public IImageEntry GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IImageEntry GetByName(string name)
        {
            var spriteSetDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (spriteSetDef == null)
                throw new Exception("No Image definition found with name: " + name);

            return spriteSetDef;
        }

        public IImageEntry GetNextTo(IImageEntry entry)
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

        public IImageEntry GetPrevTo(IImageEntry entry)
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
        public void Remove(IImageEntry entity)
        {
            throw new NotImplementedException();
        }

        public void Update(IImageEntry entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}

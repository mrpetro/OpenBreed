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

        public string Name { get { return "Images"; } }

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(IImageEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public IImageEntry GetById(string id)
        {
            var spriteSetDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (spriteSetDef == null)
                throw new Exception("No Image definition found with Id: " + id);

            return spriteSetDef;
        }

        public IImageEntry GetNextTo(IImageEntry entry)
        {
            var index = _table.Items.IndexOf((ImageDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public IImageEntry GetPreviousTo(IImageEntry entry)
        {
            var index = _table.Items.IndexOf((ImageDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }
        public void Remove(IImageEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IImageEntry entry)
        {
            var index = _table.Items.IndexOf((ImageDef)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (ImageDef)entry;
        }

        #endregion Public Methods

    }
}

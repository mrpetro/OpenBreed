using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Images;
using OpenBreed.Common.XmlDatabase.Tables.Images;
using OpenBreed.Common.Sounds;
using OpenBreed.Common.XmlDatabase.Items.Sounds;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlSoundsRepository : IRepository<ISoundEntry>
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

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(ISoundEntry entity)
        {
            throw new NotImplementedException();
        }

        public ISoundEntry GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ISoundEntry GetByName(string name)
        {
            var entry = _table.Items.FirstOrDefault(item => item.Name == name);
            if (entry == null)
                throw new Exception("No Sound entry found with name: " + name);

            return entry;
        }

        public ISoundEntry GetNextTo(ISoundEntry entry)
        {
            var index = _table.Items.IndexOf((SoundDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public ISoundEntry GetPrevTo(ISoundEntry entry)
        {
            var index = _table.Items.IndexOf((SoundDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }


        public void Remove(ISoundEntry entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ISoundEntry entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}

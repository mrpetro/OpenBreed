using OpenBreed.Common.XmlDatabase.Tables.Images;
using OpenBreed.Common.XmlDatabase.Tables.Palettes;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Tiles;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Palettes;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlPalettesRepository : IRepository<IPaletteEntity>
    {

        #region Private Fields

        private readonly DatabasePaletteTableDef _table;

        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlPalettesRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetPaletteTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntity> Entries { get { return _table.Items; } }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(IPaletteEntity entity)
        {
            throw new NotImplementedException();
        }

        public IPaletteEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IPaletteEntity GetByName(string name)
        {
            var paletteDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (paletteDef == null)
                throw new Exception("No Palette definition found with name: " + name);

            return paletteDef;
        }

        public void Remove(IPaletteEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(IPaletteEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}

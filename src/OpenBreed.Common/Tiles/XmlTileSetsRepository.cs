using OpenBreed.Common.Database.Tables.Tiles;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Tiles
{
    public class XmlTileSetsRepository : IRepository<ITileSetEntity>
    {

        #region Private Fields

        private readonly DatabaseTileSetTableDef _table;

        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlTileSetsRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetTileSetTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(ITileSetEntity entity)
        {
            throw new NotImplementedException();
        }

        public ITileSetEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ITileSetEntity GetByName(string name)
        {
            var tileSetDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (tileSetDef == null)
                throw new Exception("No Source definition found with name: " + name);

            return tileSetDef;
        }

        public void Remove(ITileSetEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ITileSetEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}

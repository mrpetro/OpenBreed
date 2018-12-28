using OpenBreed.Common.Database.Tables.Tiles;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Tiles
{
    public class TileSetsRepository : IRepository<TileSetEntity>
    {

        #region Private Fields

        private readonly DatabaseTileSetTableDef _table;

        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public TileSetsRepository(IUnitOfWork unitOfWork, XmlDatabase context)
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

        public void Add(TileSetEntity entity)
        {
            throw new NotImplementedException();
        }

        public TileSetEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public TileSetEntity GetByName(string name)
        {
            var tileSetDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (tileSetDef == null)
                throw new Exception("No Source definition found with name: " + name);

            var source = UnitOfWork.GetRepository<SourceBase>().GetByName(tileSetDef.SourceRef);
            if (source == null)
                throw new Exception("TileSet source error: " + tileSetDef.SourceRef);

            var dataFormat = _context.FormatMan.Create(source, tileSetDef.Format);

            return new TileSetEntity(this, tileSetDef.Name, dataFormat);
        }

        public void Remove(TileSetEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TileSetEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}

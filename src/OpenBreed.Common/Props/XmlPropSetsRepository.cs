using OpenBreed.Common.Database.Tables.Props;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Props
{
    public class XmlPropSetsRepository : IRepository<IPropSetEntity>
    {
        #region Private Fields

        private readonly DatabasePropertySetTableDef _table;
        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlPropSetsRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetPropSetTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntity> Entries { get { return _table.Items; } }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(IPropSetEntity entity)
        {
            throw new NotImplementedException();
        }

        public IPropSetEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IPropSetEntity GetByName(string name)
        {
            var propSetDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (propSetDef == null)
                throw new Exception("No Source definition found with name: " + name);

            return propSetDef;
        }

        public void Remove(IPropSetEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(IPropSetEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}

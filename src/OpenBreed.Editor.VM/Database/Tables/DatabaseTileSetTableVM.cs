using OpenBreed.Common;
using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabaseTileSetTableVM : DatabaseTableVM
    {
        #region Private Fields

        private IRepository<ITileSetEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseTileSetTableVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Tile sets"; } }

        #endregion Public Properties

        #region Public Methods

        public override IEnumerable<DatabaseItemVM> GetItems()
        {
            foreach (var entry in _repository.Entries)
            {
                var itemVM = Owner.CreateItem(entry);
                itemVM.Load(entry);
                yield return itemVM;
            }
        }

        public override void Load(IRepository repository)
        {
            _repository = repository as IRepository<ITileSetEntry> ?? throw new InvalidOperationException($"Expected {nameof(IRepository<ITileSetEntry>)}");
        }

        #endregion Public Methods
    }
}

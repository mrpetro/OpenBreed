using OpenBreed.Common;
using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Tables.Levels;
using OpenBreed.Common.Maps;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabaseLevelTableVM : DatabaseTableVM
    {
        #region Private Fields

        private IRepository<ILevelEntity> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseLevelTableVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Levels"; } }

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
            _repository = repository as IRepository<ILevelEntity> ?? throw new InvalidOperationException($"Expected {nameof(IRepository<ILevelEntity>)}");
        }

        #endregion Public Methods
    }
}

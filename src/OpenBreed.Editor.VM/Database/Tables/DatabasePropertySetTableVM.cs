using OpenBreed.Common;
using OpenBreed.Common.Props;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabasePropertySetTableVM : DbTableVM
    {
        #region Private Fields

        private IRepository<IPropSetEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DatabasePropertySetTableVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Property sets"; } }

        #endregion Public Properties

        #region Public Methods

        public override IEnumerable<Items.DbEntryVM> GetItems()
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
            _repository = repository as IRepository<IPropSetEntry> ?? throw new InvalidOperationException($"Expected {nameof(IRepository<IPropSetEntry>)}");
        }

        #endregion Public Methods
    }
}

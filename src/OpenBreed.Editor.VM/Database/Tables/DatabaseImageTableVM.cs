using OpenBreed.Common;
using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Tables.Images;
using OpenBreed.Common.Images;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabaseImageTableVM : DatabaseTableVM
    {

        #region Private Fields

        private IRepository<IImageEntity> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseImageTableVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Images"; } }

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
            _repository = repository as IRepository<IImageEntity> ?? throw new InvalidOperationException($"Expected {nameof(IRepository<IImageEntity>)}");
        }

        #endregion Public Methods
    }
}

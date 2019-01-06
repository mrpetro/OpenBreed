using OpenBreed.Common;
using OpenBreed.Common.Palettes;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabasePaletteTableVM : DatabaseTableVM
    {
        #region Private Fields

        private IRepository<IPaletteEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DatabasePaletteTableVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Palettes"; } }

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
            _repository = repository as IRepository<IPaletteEntry> ?? throw new InvalidOperationException($"Expected {nameof(IRepository<IPaletteEntry>)}");
        }

        #endregion Public Methods
    }
}

using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Maps;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbMapEntryVM : DbEntryVM
    {
        #region Private Fields

        private IMapEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbMapEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IMapEntry ?? throw new InvalidOperationException($"Expected {nameof(IMapEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbSpriteSetEntryVM : DbEntryVM
    {
        #region Private Fields

        private ISpriteSetEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbSpriteSetEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as ISpriteSetEntry ?? throw new InvalidOperationException($"Expected {nameof(ISpriteSetEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sounds;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbSoundEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbSound _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbSoundEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbSound ?? throw new InvalidOperationException($"Expected {nameof(IDbSound)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
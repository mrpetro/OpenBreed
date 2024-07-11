using OpenBreed.Common;
using OpenBreed.Model.Texts;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Texts;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbTextEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbText _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbTextEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbText ?? throw new InvalidOperationException($"Expected {nameof(IDbText)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
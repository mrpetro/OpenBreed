using OpenBreed.Common;
using OpenBreed.Model.Scripts;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbScriptEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbScript _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbScriptEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbScript ?? throw new InvalidOperationException($"Expected {nameof(IDbScript)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
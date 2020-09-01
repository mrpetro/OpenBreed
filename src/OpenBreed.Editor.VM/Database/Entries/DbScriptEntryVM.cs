using OpenBreed.Common;
using OpenBreed.Common.Model.Scripts;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbScriptEntryVM : DbEntryVM
    {
        #region Private Fields

        private IScriptEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbScriptEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IScriptEntry ?? throw new InvalidOperationException($"Expected {nameof(IScriptEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
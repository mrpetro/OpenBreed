using OpenBreed.Common;
using OpenBreed.Common.Texts;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbTextEntryVM : DbEntryVM
    {
        #region Private Fields

        private ITextEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbTextEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as ITextEntry ?? throw new InvalidOperationException($"Expected {nameof(ITextEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Animations;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbAnimationEntryVM : DbEntryVM
    {
        #region Private Fields

        private IAnimationEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbAnimationEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IAnimationEntry ?? throw new InvalidOperationException($"Expected {nameof(IAnimationEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
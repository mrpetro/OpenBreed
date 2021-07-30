using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbSpriteSetEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbSpriteAtlas _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbSpriteSetEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbSpriteAtlas ?? throw new InvalidOperationException($"Expected {nameof(IDbSpriteAtlas)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Props;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.XmlDatabase.Items.Sprites;
using OpenBreed.Common.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
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

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as ISpriteSetEntry ?? throw new InvalidOperationException($"Expected {nameof(ISpriteSetEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}

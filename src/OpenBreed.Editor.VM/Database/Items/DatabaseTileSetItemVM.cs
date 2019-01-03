using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Props;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.XmlDatabase.Items.Tiles;
using OpenBreed.Common.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabaseTileSetItemVM : DatabaseItemVM
    {
        #region Private Fields

        private ITileSetEntity _entry;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseTileSetItemVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntity entry)
        {
            _entry = entry as ITileSetEntity ?? throw new InvalidOperationException($"Expected {nameof(ITileSetEntity)}");

            base.Load(entry);
        }

        public override void Open()
        {
            Owner.Root.TileSetEditor.OpenEntry(_entry.Name);
            Owner.OpenedItem = this;
        }

        #endregion Public Methods
    }
}

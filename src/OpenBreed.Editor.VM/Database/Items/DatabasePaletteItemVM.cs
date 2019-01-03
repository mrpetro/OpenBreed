using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Palettes;
using OpenBreed.Common.XmlDatabase.Items.Props;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.XmlDatabase.Items.Sprites;
using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabasePaletteItemVM : DatabaseItemVM
    {

        #region Private Fields

        private IPaletteEntity _entry;

        #endregion Private Fields

        #region Public Constructors

        public DatabasePaletteItemVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntity entry)
        {
            _entry = entry as IPaletteEntity ?? throw new InvalidOperationException($"Expected {nameof(IPaletteEntity)}");

            base.Load(entry);
        }

        public override void Open()
        {
            Owner.Root.PaletteEditor.OpenEntry(_entry.Name);
            Owner.OpenedItem = this;
        }

        #endregion Public Methods

    }
}

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
    public class DbPaletteEntryVM : DbEntryVM
    {

        #region Private Fields

        private IPaletteEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbPaletteEntryVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IPaletteEntry ?? throw new InvalidOperationException($"Expected {nameof(IPaletteEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods

    }
}

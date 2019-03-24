using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Palettes;
using OpenBreed.Common.XmlDatabase.Items.Actions;
using OpenBreed.Common.XmlDatabase.Items.Assets;
using OpenBreed.Common.XmlDatabase.Items.Sprites;
using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Texts;

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

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as ITextEntry ?? throw new InvalidOperationException($"Expected {nameof(ITextEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods

    }
}

using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.XmlDatabase.Items.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteFromBinaryVM : PaletteVM
    {

        #region Private Fields

        #endregion Private Fields

        #region Public Properties


        #endregion Public Properties

        #region Internal Methods

        internal override void FromEntry(IPaletteEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IPaletteFromBinaryEntry)entry);
        }

        internal override void ToEntry(IPaletteEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IPaletteFromBinaryEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(IPaletteFromBinaryEntry entry)
        {
            DataRef = entry.DataRef;
        }

        private void ToEntry(IPaletteFromBinaryEntry entry)
        {
            entry.DataRef = DataRef;
        }

        #endregion Private Methods

    }
}

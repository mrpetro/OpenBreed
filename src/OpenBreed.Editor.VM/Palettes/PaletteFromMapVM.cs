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
    public class PaletteFromMapVM : PaletteVM
    {

        #region Private Fields

        private string _blockName;

        #endregion Private Fields

        #region Public Properties

        public string BlockName
        {
            get { return _blockName; }
            set { SetProperty(ref _blockName, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IPaletteFromMapEntry)entry);
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IPaletteFromMapEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(IPaletteFromMapEntry source)
        {
            DataRef = source.DataRef;
            BlockName = source.BlockName;

        }

        private void ToEntry(IPaletteFromMapEntry source)
        {
            source.DataRef = DataRef;
            source.BlockName = BlockName;
        }

        #endregion Private Methods

    }
}

using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Maps
{
    public class MapPaletteDataBlock : IMapDataBlock
    {

        #region Public Constructors

        public MapPaletteDataBlock(string name, PaletteModel value = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Length { get { return Value.Length; } }
        public string Name { get; }
        public PaletteModel Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        #endregion Public Methods

    }
}

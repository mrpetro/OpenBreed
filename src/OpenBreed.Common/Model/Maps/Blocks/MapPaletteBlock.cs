using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Model.Maps.Blocks
{
    public class MapPaletteBlock : IMapDataBlock
    {

        #region Public Constructors

        public MapPaletteBlock(string name, ColorData[] value = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; }

        public ColorData[] Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        #endregion Public Methods

        #region Public Structs

        public struct ColorData
        {
            #region Public Constructors

            public ColorData(byte r, byte g, byte b)
            {
                R = r;
                G = g;
                B = b;
            }

            #endregion Public Constructors

            #region Public Properties

            public byte B { get; set; }
            public byte G { get; set; }
            public byte R { get; set; }

            #endregion Public Properties

        }

        #endregion Public Structs

    }
}

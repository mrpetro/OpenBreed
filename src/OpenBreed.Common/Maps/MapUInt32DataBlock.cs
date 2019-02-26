using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Maps
{
    public class MapUInt32DataBlock : IMapDataBlock
    {

        #region Public Constructors

        public MapUInt32DataBlock(string name, UInt32 value = 0)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Length { get { return sizeof(int); } }
        public string Name { get; }
        public UInt32 Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        #endregion Public Methods

    }
}

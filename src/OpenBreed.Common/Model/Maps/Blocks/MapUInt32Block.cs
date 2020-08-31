using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Model.Maps.Blocks
{
    public class MapUInt32Block : IMapDataBlock
    {

        #region Public Constructors

        public MapUInt32Block(string name, UInt32 value = 0)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

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

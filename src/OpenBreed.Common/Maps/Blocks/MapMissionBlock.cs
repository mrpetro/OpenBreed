using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Maps.Blocks
{
    public class MapMissionBlock : IMapDataBlock
    {

        #region Public Constructors

        public MapMissionBlock(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; }

        public int EXC1 { get; set; }
        public int EXC2 { get; set; }
        public int EXC3 { get; set; }
        public int EXC4 { get; set; }
        public string LCTX { get; set; }
        public int M1HE { get; set; }
        public int M1SP { get; set; }
        public int M1TY { get; set; }
        public int M2HE { get; set; }
        public int M2SP { get; set; }
        public int M2TY { get; set; }
        public string MTXT { get; set; }
        public string NOT1 { get; set; }
        public string NOT2 { get; set; }
        public string NOT3 { get; set; }
        public int TIME { get; set; }
        public int UNKN1 { get; set; }
        public int UNKN16 { get; set; }
        public int UNKN17 { get; set; }
        public int UNKN2 { get; set; }
        public int UNKN21 { get; set; }
        public int UNKN22 { get; set; }
        public int UNKN3 { get; set; }
        public int UNKN4 { get; set; }
        public int UNKN6 { get; set; }
        public int UNKN7 { get; set; }
        public int UNKN8 { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"{Name}";
        }

        #endregion Public Methods

    }
}

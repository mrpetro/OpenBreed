using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Maps.Builders
{
    public class MapMissionBuilder
    {
        internal UInt16 UNKN1;
        internal UInt16 UNKN2;
        internal UInt16 UNKN3;
        internal UInt16 UNKN4;
        internal int TIME;
        internal UInt16 UNKN6;
        internal UInt16 UNKN7;
        internal UInt16 UNKN8;
        internal int EXC1;
        internal int EXC2;
        internal int EXC3;
        internal int EXC4;
        internal int M1TY;
        internal int M1HE;
        internal int M1SP;
        internal UInt16 UNKN16;
        internal UInt16 UNKN17;
        internal int M2TY;
        internal int M2HE;
        internal int M2SP;
        internal UInt16 UNKN21;
        internal UInt16 UNKN22;

        internal string MTXT;
        internal string LCTX;
        internal string NOT1;
        internal string NOT2;
        internal string NOT3;

        public static MapMissionBuilder NewMissionModel()
        {
            return new MapMissionBuilder();
        }

        public MapMissionModel Build()
        {
            return new MapMissionModel(this);
        }
    }
}

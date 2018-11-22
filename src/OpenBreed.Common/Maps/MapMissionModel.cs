using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Common.Maps.Builders;

namespace OpenBreed.Common.Maps
{
    public class MapMissionModel
    {
        #region Public Constructors

        public MapMissionModel(MapMissionBuilder builder)
        {
            UNKN1 = builder.UNKN1;
            UNKN2 = builder.UNKN2;
            UNKN3 = builder.UNKN3;
            UNKN4 = builder.UNKN4;
            TIME = builder.TIME;
            UNKN6 = builder.UNKN6;
            UNKN7 = builder.UNKN7;
            UNKN8 = builder.UNKN8;
            EXC1 = builder.EXC1;
            EXC2 = builder.EXC2;
            EXC3 = builder.EXC3;
            EXC4 = builder.EXC4;
            M1TY = builder.M1TY;
            M1HE = builder.M1HE;
            M1SP = builder.M1SP;
            UNKN16 = builder.UNKN16;
            UNKN17 = builder.UNKN17;
            M2TY = builder.M2TY;
            M2HE = builder.M2HE;
            M2SP = builder.M2SP;
            UNKN21 = builder.UNKN21;
            UNKN22 = builder.UNKN22;

            MTXT = builder.MTXT;
            LCTX = builder.LCTX;
            NOT1 = builder.NOT1;
            NOT2 = builder.NOT2;
            NOT3 = builder.NOT3;
        }

        #endregion Public Constructors

        #region Public Properties

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
        public MapModel Owner { get; internal set; }
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
    }
}

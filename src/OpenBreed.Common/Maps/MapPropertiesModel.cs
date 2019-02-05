using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Maps.Builders;

namespace OpenBreed.Common.Maps
{
    public class MapPropertiesModel
    {

        #region Public Constructors

        public MapPropertiesModel(MapPropertiesBuilder builder)
        {
            Header = builder.Header;
            XBLK = builder.XBLK;
            YBLK = builder.YBLK;
            XOFC = builder.XOFC;
            YOFC = builder.YOFC;
            XOFM = builder.XOFM;
            YOFM = builder.YOFM;
            XOFA = builder.XOFA;
            YOFA = builder.YOFA;
            XOFC = builder.XOFC;
            YOFC = builder.YOFC;
            IFFP = builder.IFFP;
            ALTM = builder.ALTM;
            ALTP = builder.ALTP;
            CCCI = builder.CCCI;
            CCIN = builder.CCIN;
            CSIN = builder.CSIN;

            Palettes = builder.Palettes;
        }

        #endregion Public Constructors

        #region Public Properties

        public string ALTM { get; set; }
        public string ALTP { get; set; }
        public byte[] CCCI { get; set; }
        public byte[] CCIN { get; set; }
        public byte[] CSIN { get; set; }
        public byte[] Header { get; set; }
        public string IFFP { get; set; }
        public List<PaletteModel> Palettes { get; set; }
        public int XBLK { get; set; }
        public int XOFA { get; set; }
        public int XOFC { get; set; }
        public int XOFM { get; set; }
        public int YBLK { get; set; }
        public int YOFA { get; set; }
        public int YOFC { get; set; }
        public int YOFM { get; set; }

        #endregion Public Properties

    }
}

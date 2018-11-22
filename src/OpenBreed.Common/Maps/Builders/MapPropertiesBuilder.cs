using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Common.Palettes;

namespace OpenBreed.Common.Maps.Builders
{
    public class MapPropertiesBuilder
    {
        internal byte[] Header;
        internal int XBLK;
        internal int YBLK;
        internal int XOFC;
        internal int YOFC;
        internal int XOFM;
        internal int YOFM;
        internal int XOFA;
        internal int YOFA;
        internal string IFFP;
        internal string ALTM;
        internal string ALTP;
        internal byte[] CCCI;
        internal byte[] CCIN;
        internal byte[] CSIN;
        internal List<PaletteModel> Palettes;

        private MapPropertiesBuilder()
        {
            Palettes = new List<PaletteModel>();
        }

        public static MapPropertiesBuilder NewPropertiesModel()
        {
            return new MapPropertiesBuilder();
        }

        public MapPropertiesBuilder SetHeader(byte[] header)
        {
            Header = header;

            return this;
        }

        public void AddPalette(PaletteModel palette)
        {
            Palettes.Add(palette);
        }

        public MapPropertiesBuilder SetBLK(int width, int height)
        {
            XBLK = width;
            YBLK = height;

            return this;
        }

        public MapPropertiesBuilder SetOFC(int x, int y)
        {
            XOFC = x;
            YOFC = y;

            return this;
        }

        public MapPropertiesBuilder SetOFM(int x, int y)
        {
            XOFM = x;
            YOFM = y;

            return this;
        }


        public MapPropertiesBuilder SetOFA(int x, int y)
        {
            XOFA = x;
            YOFA = y;

            return this;
        }

        public MapPropertiesBuilder SetIFFP(string iffp)
        {
            IFFP = iffp;

            return this;
        }

        public MapPropertiesModel Build()
        {
            return new MapPropertiesModel(this);
        } 
    }
}

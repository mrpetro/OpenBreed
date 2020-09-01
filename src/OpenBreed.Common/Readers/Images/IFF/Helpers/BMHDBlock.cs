using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Readers.Images.IFF.Helpers
{
    /// <summary>
    /// BMHD: Bitmap Header user in IFF image data
    /// Implementation source: https://en.wikipedia.org/wiki/ILBM
    /// </summary>
    public class BMHDBlock
    {
        public enum MaskEnum : byte
        {
            None = 0,
            Masked = 1,
            TransparentColor = 2,
            Lasso = 3
        }

        public enum CompressionEnum : byte
        {
            Uncompressed = 0,
            RLECompressed = 1,
            VerticalRLE = 2,
        }

        public UInt16 Width { get; set; }
        public UInt16 Height { get; set; }
        public Int16 Xorigin { get; set; }
        public Int16 Yorigin { get; set; }
        public byte NumPlanes { get; set; }
        public MaskEnum Mask { get; set; }
        public CompressionEnum Compression { get; set; }
        public byte Pad1 { get; set; }
        public UInt16 TransClr { get; set; }
        public byte Xaspect { get; set; }
        public byte Yaspect { get; set; }
        public Int16 PageWidth { get; set; }
        public Int16 PageHeight { get; set; }
    }
}

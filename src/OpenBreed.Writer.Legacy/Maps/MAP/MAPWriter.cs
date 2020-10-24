using OpenBreed.Common.Tools;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Reader.Legacy.Maps.MAP;
using OpenBreed.Writer.Interface.Maps;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenBreed.Writer.Legacy.Maps.MAP
{
    public class MAPWriter : IMapModelWriter, IDisposable
    {
        #region Public Fields

        public readonly MAPFormat Format;

        #endregion Public Fields

        #region Private Fields

        private const string XBLK = "XBLK";
        private const string YBLK = "YBLK";
        private const string XOFC = "XOFC";
        private const string YOFC = "YOFC";
        private const string XOFM = "XOFM";
        private const string YOFM = "YOFM";
        private const string XOFA = "XOFA";
        private const string YOFA = "YOFA";
        private const string IFFP = "IFFP";
        private const string ALTM = "ALTM";
        private const string ALTP = "ALTP";
        private const string CMAP = "CMAP";
        private const string ALCM = "ALCM";
        private const string CCCI = "CCCI";
        private const string CCIN = "CCIN";
        private const string CSIN = "CSIN";
        private const string MTXT = "MTXT";
        private const string LCTX = "LCTX";
        private const string NOT1 = "NOT1";
        private const string NOT2 = "NOT2";
        private const string NOT3 = "NOT3";
        private const string BODY = "BODY";

        private readonly BigEndianBinaryWriter _binWriter;

        private readonly CellValueGetter GetCellValue;

        #endregion Private Fields

        #region Public Constructors

        public MAPWriter(Stream stream, MAPFormat format)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Format = format;

            if (Format == MAPFormat.ABSE)
                GetCellValue = GetABSEBodyCellValue;
            else
                GetCellValue = GetABTABodyCellValue;

            //Don't call Dispode of this Writer for stream to stay opened
            _binWriter = new BigEndianBinaryWriter(stream);
        }

        #endregion Public Constructors

        #region Private Delegates

        private delegate int CellValueGetter(int actionId, int gfxId);

        #endregion Private Delegates

        #region Public Methods

        public void Dispose()
        {
        }

        public void Write(MapModel map)
        {
            WriteHeader(map);

            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == XBLK));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == YBLK));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == XOFC));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == YOFC));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == XOFM));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == YOFM));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == XOFA));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == YOFA));

            WriteStringBlock(map.Blocks.OfType<MapStringBlock>().First(item => item.Name == IFFP));
            WriteStringBlock(map.Blocks.OfType<MapStringBlock>().First(item => item.Name == ALTM));
            WriteStringBlock(map.Blocks.OfType<MapStringBlock>().First(item => item.Name == ALTP));

            WritePaletteBlock(map.Blocks.OfType<MapPaletteBlock>().First(item => item.Name == CMAP));
            WritePaletteBlock(map.Blocks.OfType<MapPaletteBlock>().First(item => item.Name == ALCM));

            WriteBytesBlock(map.Blocks.OfType<MapUnknownBlock>().First(item => item.Name == CCCI));
            WriteBytesBlock(map.Blocks.OfType<MapUnknownBlock>().First(item => item.Name == CCIN));
            WriteBytesBlock(map.Blocks.OfType<MapUnknownBlock>().First(item => item.Name == CSIN));

            var missionBlock = map.Blocks.OfType<MapMissionBlock>().FirstOrDefault();
            if (missionBlock != null)
                WriteMissionBlock(missionBlock);

            var mtxtBlock = map.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == MTXT);
            if (mtxtBlock != null)
                WriteTextBlock(mtxtBlock);

            var lctxBlock = map.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == LCTX);
            if (lctxBlock != null)
                WriteTextBlock(lctxBlock);

            var not1Block = map.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == NOT1);
            if (not1Block != null)
                WriteTextBlock(not1Block);

            var not2Block = map.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == NOT2);
            if (not2Block != null)
                WriteTextBlock(not2Block);

            var not3Block = map.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == NOT3);
            if (not3Block != null)
                WriteTextBlock(not3Block);

            WriteBodyBlock(map);
        }

        #endregion Public Methods

        #region Private Methods

        private int GetABSEBodyCellValue(int actionId, int gfxId)
        {
            return (gfxId << 9) | (actionId << 7) >> 7;
        }

        private int GetABTABodyCellValue(int actionId, int gfxId)
        {
            return (gfxId << 6) | (actionId << 10) >> 10;
        }

        private void WriteBodyBlock(MapModel map)
        {
            var layout = map.Layout;
            var blockName = BODY;
            var blockSize = layout.Width * layout.Height * 2;
            var actionLayerIndex = layout.GetLayerIndex(MapLayerType.Action);
            var gfxLayerIndex = layout.GetLayerIndex(MapLayerType.Gfx);

            _binWriter.Write(Encoding.ASCII.GetBytes(blockName));
            _binWriter.Write(Convert.ToUInt32(blockSize));

            for (int y = 0; y < layout.Height; y++)
            {
                for (int x = 0; x < layout.Width; x++)
                {
                    var cellValues = layout.GetCellValues(x, y);
                    var actionId = cellValues[actionLayerIndex];
                    var gfxId = cellValues[gfxLayerIndex];
                    var value = GetCellValue(actionId, gfxId);
                    _binWriter.Write(Convert.ToUInt16(value));
                }
            }
        }

        private void WriteBytesBlock(MapUnknownBlock block)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes(block.Name));
            _binWriter.Write((UInt32)block.Value.Length);
            _binWriter.Write(block.Value);
        }

        private void WriteHeader(MapModel map)
        {
            if (map.Header.Length != 12)
                throw new Exception("Header has wrong size (not 12 bytes).");

            byte[] validHeader = new byte[12];

            Array.Copy(map.Header, validHeader, map.Header.Length > 12 ? 12 : map.Header.Length);

            //Write file header. This should contain editor name
            _binWriter.Write(validHeader);
        }

        private void WriteMissionBlock(MapMissionBlock block)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes(block.Name));
            //For some reason this block has to have length overlapping with start of next block
            _binWriter.Write((UInt32)48);

            _binWriter.Write((UInt16)block.UNKN1);
            _binWriter.Write((UInt16)block.UNKN2);
            _binWriter.Write((UInt16)block.UNKN3);
            _binWriter.Write((UInt16)block.UNKN4);
            _binWriter.Write((UInt16)block.TIME);
            _binWriter.Write((UInt16)block.UNKN6);
            _binWriter.Write((UInt16)block.UNKN7);
            _binWriter.Write((UInt16)block.UNKN8);
            _binWriter.Write((UInt16)block.EXC1);
            _binWriter.Write((UInt16)block.EXC2);
            _binWriter.Write((UInt16)block.EXC3);
            _binWriter.Write((UInt16)block.EXC4);
            _binWriter.Write((UInt16)block.M1TY);
            _binWriter.Write((UInt16)block.M1HE);
            _binWriter.Write((UInt16)block.M1SP);
            _binWriter.Write((UInt16)block.UNKN16);
            _binWriter.Write((UInt16)block.UNKN17);
            _binWriter.Write((UInt16)block.M2TY);
            _binWriter.Write((UInt16)block.M2HE);
            _binWriter.Write((UInt16)block.M2SP);
            _binWriter.Write((UInt16)block.UNKN21);
            _binWriter.Write((UInt16)block.UNKN22);
        }

        private void WritePaletteBlock(MapPaletteBlock block)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes(block.Name));
            _binWriter.Write((UInt32)(block.Value.Length * 3));

            foreach (var color in block.Value)
            {
                _binWriter.Write(color.R);
                _binWriter.Write(color.G);
                _binWriter.Write(color.B);
            }
        }

        private void WriteString(string value)
        {
            var newValue = Other.ConvertLineBreaksCRLFToLF(value);
            _binWriter.Write(Encoding.ASCII.GetBytes(newValue));
        }

        private void WriteStringBlock(MapStringBlock block)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes(block.Name));

            var bytes = Encoding.ASCII.GetBytes(block.Value);
            _binWriter.Write((UInt32)bytes.Length);
            _binWriter.Write(bytes);
        }

        /// <summary>
        /// This writes a MAP file text block.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text">Text to write in to block</param>
        private void WriteTextBlock(MapTextBlock block)
        {
            var text = block.Value;

            text = Other.ConvertLineBreaksCRLFToLF(text) + "\0";
            //Insure division by 2 of block length
            text = text.PadRight(text.Length + text.Length % 2, '\0');

            _binWriter.Write(Encoding.ASCII.GetBytes(block.Name));
            //For some reason each text block has to have length overlapping with start of next block
            _binWriter.Write((UInt32)(text.Length + 4));
            _binWriter.Write(Encoding.ASCII.GetBytes(text));
        }

        private void WriteUInt32Block(MapUInt32Block block)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes(block.Name));
            _binWriter.Write((UInt32)4);
            _binWriter.Write(block.Value);
        }

        #endregion Private Methods
    }
}
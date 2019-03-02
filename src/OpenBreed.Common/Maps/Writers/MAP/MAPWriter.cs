using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Writers;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Maps.Blocks;
using OpenBreed.Common.Maps.Readers.MAP;

namespace OpenBreed.Common.Maps.Writers.MAP
{
    public class MAPWriter : IMapModelWriter, IDisposable
    {
        #region Private Fields

        private readonly BigEndianBinaryWriter _binWriter;

        #endregion Private Fields

        public readonly MAPFormat Format;


        #region Public Constructors

        public MAPWriter(Stream stream, MAPFormat format)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Format = format;

            //Don't call Dispode of this Writer for stream to stay opened
            _binWriter = new BigEndianBinaryWriter(stream);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {

        }

        public void Write(MapModel map)
        {
            WriteHeader(map);


            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == "XBLK"));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == "YBLK"));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == "XOFC"));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == "YOFC"));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == "XOFM"));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == "YOFM"));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == "XOFA"));
            WriteUInt32Block(map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == "YOFA"));

            WriteStringBlock(map.Blocks.OfType<MapStringBlock>().First(item => item.Name == "IFFP"));
            WriteStringBlock(map.Blocks.OfType<MapStringBlock>().First(item => item.Name == "ALTM"));
            WriteStringBlock(map.Blocks.OfType<MapStringBlock>().First(item => item.Name == "ALTP"));

            WritePaletteBlock(map.Blocks.OfType<MapPaletteBlock>().First(item => item.Name == "CMAP"));
            WritePaletteBlock(map.Blocks.OfType<MapPaletteBlock>().First(item => item.Name == "ALCM"));

            WriteBytesBlock(map.Blocks.OfType<MapUnknownBlock>().First(item => item.Name == "CCCI"));
            WriteBytesBlock(map.Blocks.OfType<MapUnknownBlock>().First(item => item.Name == "CCIN"));
            WriteBytesBlock(map.Blocks.OfType<MapUnknownBlock>().First(item => item.Name == "CSIN"));

            var missionBlock = map.Blocks.OfType<MapMissionBlock>().FirstOrDefault();
            if (missionBlock != null)
                WriteMissionBlock(missionBlock);

            var mtxtBlock = map.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == "MTXT");
            if (mtxtBlock != null)
                WriteTextBlock(mtxtBlock);

            var lctxBlock = map.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == "LCTX");
            if (lctxBlock != null)
                WriteTextBlock(lctxBlock);

            var not1Block = map.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == "NOT1");
            if (not1Block != null)
                WriteTextBlock(not1Block);

            var not2Block = map.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == "NOT2");
            if (not2Block != null)
                WriteTextBlock(not2Block);

            var not3Block = map.Blocks.OfType<MapTextBlock>().FirstOrDefault(item => item.Name == "NOT3");
            if (not3Block != null)
                WriteTextBlock(not3Block);

            WriteBodyBlock(map.Blocks.OfType<MapBodyBlock>().First(item => item.Name == "BODY"));
        }

        #endregion Public Methods

        #region Private Methods

        private void WriteBodyBlock(MapBodyBlock block)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes(block.Name));
            _binWriter.Write((UInt32)(block.Cells.Length * 2));

            if (Format == MAPFormat.ABSE)
            {
                for (int i = 0; i < block.Cells.Length; i++)
                {
                    var gfxId = block.Cells[i].GfxId;
                    var actionId = block.Cells[i].ActionId;
                    var value = (UInt16)((gfxId << 9) | (actionId << 7) >> 7);
                    _binWriter.Write(value);
                }
            }
            else
            {
                for (int i = 0; i < block.Cells.Length; i++)
                {
                    var gfxId = block.Cells[i].GfxId;
                    var actionId = block.Cells[i].ActionId;
                    var value = (UInt16)((gfxId << 6) | (actionId << 10) >> 10);
                    _binWriter.Write(value);
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
                LogMan.Instance.LogWarning("Header has wrong size (not 12 bytes). Adjusted.");

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Writers;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Maps.Blocks;

namespace OpenBreed.Common.Maps.Writers.MAP
{
    public class MAPWriter : IMapModelWriter, IDisposable
    {
        #region Private Fields

        private readonly BigEndianBinaryWriter _binWriter;

        #endregion Private Fields

        #region Public Constructors

        public MAPWriter(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

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


            //WriteUInt32Block("XBLK", (UInt32)map.Properties.XBLK);
            //WriteUInt32Block("YBLK", (UInt32)map.Properties.YBLK);
            //WriteUInt32Block("XOFC", (UInt32)map.Properties.XOFC);
            //WriteUInt32Block("YOFC", (UInt32)map.Properties.YOFC);
            //WriteUInt32Block("XOFM", (UInt32)map.Properties.XOFM);
            //WriteUInt32Block("YOFM", (UInt32)map.Properties.YOFM);
            //WriteUInt32Block("XOFA", (UInt32)map.Properties.XOFA);
            //WriteUInt32Block("YOFA", (UInt32)map.Properties.YOFA);

            //WriteStringBlock("IFFP", map.Properties.IFFP);
            //WriteStringBlock("ALTM", map.Properties.ALTM);
            //WriteStringBlock("ALTP", map.Properties.ALTP);

            //WritePaletteBlock("CMAP", map.Properties.Palettes.FirstOrDefault(item => item.Name == "CMAP"));
            //WritePaletteBlock("ALCM", map.Properties.Palettes.FirstOrDefault(item => item.Name == "ALCM"));

            //WriteBytesBlock("CCCI", map.Properties.CCCI);
            //WriteBytesBlock("CCIN", map.Properties.CCIN);
            //WriteBytesBlock("CSIN", map.Properties.CSIN);

            //WriteMission(map.Mission);

            //WriteBody(map.Layout);
        }

        #endregion Public Methods

        #region Private Methods

        private void WriteBody(MapBodyBlock bodyBlock)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes("BODY"));
            _binWriter.Write((UInt32)(bodyBlock.Length * 2));

            for (int i = 0; i < bodyBlock.Length; i++)
            {
                var gfxId = bodyBlock.Cells[i].GfxId;
                var actionId = bodyBlock.Cells[i].ActionId;
                var value = (UInt16)((gfxId << 6) | (actionId << 10) >> 10);
                _binWriter.Write(value);
            }
        }

        private void WriteBytesBlock(string blockName, byte[] value)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes(blockName));
            _binWriter.Write((UInt32)value.Length);
            _binWriter.Write(value);
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

        private void WriteMission(MapMissionBlock mission)
        {
            WriteMissionBlock(mission);

            if (!string.IsNullOrEmpty(mission.MTXT))
                WriteTextBlock("MTXT", mission.MTXT);

            if (!string.IsNullOrEmpty(mission.LCTX))
                WriteTextBlock("LCTX", mission.LCTX);

            if (!string.IsNullOrEmpty(mission.NOT1))
                WriteTextBlock("NOT1", mission.NOT1);

            if (!string.IsNullOrEmpty(mission.NOT2))
                WriteTextBlock("NOT2", mission.NOT2);

            if (!string.IsNullOrEmpty(mission.NOT3))
                WriteTextBlock("NOT3", mission.NOT3);
        }

        /// <summary>
        /// This writes a MAP file mission block.
        /// </summary>
        /// <param name="mission"></param>
        private void WriteMissionBlock(MapMissionBlock mission)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes("MISS"));
            //For some reason this block has to have length overlapping with start of next block
            _binWriter.Write((UInt32)48);

            _binWriter.Write((UInt16)mission.UNKN1);
            _binWriter.Write((UInt16)mission.UNKN2);
            _binWriter.Write((UInt16)mission.UNKN3);
            _binWriter.Write((UInt16)mission.UNKN4);
            _binWriter.Write((UInt16)mission.TIME);
            _binWriter.Write((UInt16)mission.UNKN6);
            _binWriter.Write((UInt16)mission.UNKN7);
            _binWriter.Write((UInt16)mission.UNKN8);
            _binWriter.Write((UInt16)mission.EXC1);
            _binWriter.Write((UInt16)mission.EXC2);
            _binWriter.Write((UInt16)mission.EXC3);
            _binWriter.Write((UInt16)mission.EXC4);
            _binWriter.Write((UInt16)mission.M1TY);
            _binWriter.Write((UInt16)mission.M1HE);
            _binWriter.Write((UInt16)mission.M1SP);
            _binWriter.Write((UInt16)mission.UNKN16);
            _binWriter.Write((UInt16)mission.UNKN17);
            _binWriter.Write((UInt16)mission.M2TY);
            _binWriter.Write((UInt16)mission.M2HE);
            _binWriter.Write((UInt16)mission.M2SP);
            _binWriter.Write((UInt16)mission.UNKN21);
            _binWriter.Write((UInt16)mission.UNKN22);
        }

        private void WritePaletteBlock(string blockName, PaletteModel palette)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes(blockName));
            _binWriter.Write((UInt32)(palette.Length * 3));

            foreach (var color in palette.Data)
            {
                _binWriter.Write((byte)color.R);
                _binWriter.Write((byte)color.G);
                _binWriter.Write((byte)color.B);
            }
        }

        private void WriteString(string value)
        {
            var newValue = Other.ConvertLineBreaksCRLFToLF(value);
            _binWriter.Write(Encoding.ASCII.GetBytes(newValue));
        }

        private void WriteStringBlock(string blockName, string value)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes(blockName));

            var bytes = Encoding.ASCII.GetBytes(value);
            _binWriter.Write((UInt32)bytes.Length);
            _binWriter.Write(bytes);
        }

        /// <summary>
        /// This writes a MAP file text block.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text">Text to write in to block</param>
        private void WriteTextBlock(string name, string text)
        {
            text = Other.ConvertLineBreaksCRLFToLF(text) + "\0";
            //Insure division by 2 of block length
            text = text.PadRight(text.Length + text.Length % 2, '\0');

            _binWriter.Write(Encoding.ASCII.GetBytes(name));
            //For some reason each text block has to have length overlapping with start of next block
            _binWriter.Write((UInt32)(text.Length + 4));
            _binWriter.Write(Encoding.ASCII.GetBytes(text));
        }

        private void WriteUInt32Block(string blockName, UInt32 value)
        {
            _binWriter.Write(Encoding.ASCII.GetBytes(blockName));
            _binWriter.Write((UInt32)4);
            _binWriter.Write(value);
        }

        #endregion Private Methods
    }
}

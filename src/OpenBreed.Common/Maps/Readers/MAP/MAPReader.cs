using OpenBreed.Common.Maps.Builders;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Readers;
using System;
using System.IO;
using System.Linq;

namespace OpenBreed.Common.Maps.Readers.MAP
{
    public enum MAPFormat
    {
        ABSE,
        ABHC,
        ABTA
    }

    public class MAPReader : IMapModelReader, IDisposable
    {
        #region Public Fields

        public readonly MAPFormat Format;

        #endregion Public Fields

        #region Public Constructors

        public MAPReader(MapBuilder builder, MAPFormat format)
        {
            MapBuilder = builder;
            Format = format;
            MissionBuilder = MapMissionBuilder.NewMissionModel();
            PaletteReader = new MAPPaletteReader(this);
        }

        #endregion Public Constructors

        #region Internal Properties

        internal MapBuilder MapBuilder { get; private set; }
        internal MapMissionBuilder MissionBuilder { get; private set; }
        internal MAPPaletteReader PaletteReader { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void Dispose()
        {
        }

        private void ReadBlock(BigEndianBinaryReader binReader)
        {
            string blockName = ReadString(binReader, 4);

            switch (blockName)
            {
                default:
                    break;
            }
        }

        public MapModel Read(Stream stream)
        {
            //We dont want to close the stream here so reader is not being used inside of 'using' statement
            BigEndianBinaryReader binReader = new BigEndianBinaryReader(stream);

            ReadHeader(binReader);

            int unknownBlocks = 0;

            while (!IsEOF(binReader))
            {
                string string_result = ReadString(binReader, 4);

                if (string_result == "XBLK")
                    ReadUInt32Block("XBLK", binReader);
                else if (string_result == "YBLK")
                    ReadUInt32Block("YBLK", binReader);
                else if (string_result == "XOFC")
                    ReadUInt32Block("XOFC", binReader);
                else if (string_result == "YOFC")
                    ReadUInt32Block("YOFC", binReader);
                else if (string_result == "XOFM")
                    ReadUInt32Block("XOFM", binReader);
                else if (string_result == "YOFM")
                    ReadUInt32Block("YOFM", binReader);
                else if (string_result == "XOFA")
                    ReadUInt32Block("XOFA", binReader);
                else if (string_result == "YOFA")
                    ReadUInt32Block("YOFA", binReader);
                else if (string_result == "IFFP")
                    ReadStringBlock("IFFP", binReader);
                else if (string_result == "ALTM")
                    ReadStringBlock("ALTM", binReader);
                else if (string_result == "ALTP")
                    ReadStringBlock("ALTP", binReader);
                else if (string_result == "CMAP")
                    ReadPaletteBlock("CMAP", binReader);
                else if (string_result == "ALCM")
                    ReadPaletteBlock("ALCM", binReader);
                else if (string_result == "IFFC")
                    ReadBytesBlock("IFFC", binReader);
                else if (string_result == "CCCI")
                    ReadBytesBlock("CCCI", binReader);
                else if (string_result == "CCIN")
                    ReadBytesBlock("CCIN", binReader);
                else if (string_result == "CSIN")
                    ReadBytesBlock("CSIN", binReader);
                else if (string_result == "MISS")
                    ReadMission(binReader);
                else if (string_result == "MTXT")
                {
                    UInt32 uint_size = binReader.ReadUInt32();
                    uint_size -= 4;
                    MissionBuilder.MTXT = ReadString(binReader, uint_size);
                }
                else if (string_result == "LCTX")
                {
                    UInt32 uint_size = binReader.ReadUInt32();
                    uint_size -= 4;
                    MissionBuilder.LCTX = ReadString(binReader, uint_size);
                }
                else if (string_result == "NOT1")
                {
                    UInt32 uint_size = binReader.ReadUInt32();
                    uint_size -= 4;
                    MissionBuilder.NOT1 = ReadString(binReader, uint_size);
                }
                else if (string_result == "NOT2")
                {
                    UInt32 uint_size = binReader.ReadUInt32();
                    uint_size -= 4;
                    MissionBuilder.NOT2 = ReadString(binReader, uint_size);
                }
                else if (string_result == "NOT3")
                {
                    UInt32 uint_size = binReader.ReadUInt32();
                    uint_size -= 4;
                    MissionBuilder.NOT3 = ReadString(binReader, uint_size);
                }
                else if (string_result == "BODY")
                    ReadBodyBlock(binReader);
                else
                    ReadBytesBlock("UKWN", binReader);
                //throw new Exception(string_result + ": Unknown block!");
            }

            MapBuilder.SetMission(MissionBuilder.Build());

            return MapBuilder.Build();
        }

        #endregion Public Methods

        #region Internal Methods

        internal bool IsEOF(BigEndianBinaryReader binReader)
        {
            return binReader.BaseStream.Position == binReader.BaseStream.Length;
        }
        internal string ReadString(BigEndianBinaryReader binReader, UInt32 size)
        {
            byte[] tvByteArray = binReader.ReadBytes((int)size);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return Other.ConvertLineBreaksLFToCRLF(enc.GetString(tvByteArray));
        }

        #endregion Internal Methods

        #region Private Methods

        private void ReadBodyBlock(BigEndianBinaryReader binReader)
        {
            int sizeX = (int)MapBuilder.Blocks.OfType<MapUInt32DataBlock>().FirstOrDefault(item => item.Name == "XBLK").Value;
            int sizeY = (int)MapBuilder.Blocks.OfType<MapUInt32DataBlock>().FirstOrDefault(item => item.Name == "YBLK").Value;

            //Check how many tile can be read from file and how many are expected based on map sizes
            var tilesNo = (int)(binReader.ReadUInt32() / 2);
            int expectedTilesNo = sizeX * sizeY;

            if (tilesNo != expectedTilesNo)
                throw new Exception("Incorrect number of tiles in body (" + tilesNo + "). Expected: " + expectedTilesNo);

            var bodyBlock = new MapBodyDataBlock(tilesNo);

            for (int i = 0; i < tilesNo; i++)
            {
                UInt16 data = binReader.ReadUInt16();
                int gfxId;
                int actionId;

                if (Format == MAPFormat.ABSE)
                {
                    gfxId = (UInt16)(data << 7) >> 7;
                    actionId = data >> 9;
                }
                else
                {
                    actionId = (UInt16)(data << 10) >> 10;
                    gfxId = data >> 6;
                }

                bodyBlock.Cells[i].GfxId = gfxId;
                bodyBlock.Cells[i].ActionId = actionId;
            }

            MapBuilder.AddBlock(bodyBlock);
        }

        private void ReadBytesBlock(string name, BigEndianBinaryReader binReader)
        {
            UInt32 size = binReader.ReadUInt32();
            var value = binReader.ReadBytes((int)size);
            MapBuilder.AddBlock(new MapUnknownDataBlock(name, value));
        }

        private void ReadPaletteBlock(string name, BigEndianBinaryReader binReader)
        {
            var value = PaletteReader.Read(binReader);
            value.Name = name;
            MapBuilder.AddBlock(new MapPaletteDataBlock(name, value));
        }

        private PaletteModel ReadColorBlockEx(BigEndianBinaryReader binReader, string name)
        {
            PaletteModel result = PaletteReader.ReadEx(binReader);
            result.Name = name;
            return result;
        }

        private void ReadHeader(BigEndianBinaryReader binReader)
        {
            //Read file header. This should contain editor name
            var header = binReader.ReadBytes(12);
            MapBuilder.SetHeader(header);
        }

        private void ReadMission(BigEndianBinaryReader binReader)
        {
            UInt32 uint_size = binReader.ReadUInt32();

            //NOTE: For some reason there is 4 bytes reading offset when MISS block is used in ABTA map files.
            uint_size -= 4;

            MissionBuilder.UNKN1 = binReader.ReadUInt16();
            MissionBuilder.UNKN2 = binReader.ReadUInt16();
            MissionBuilder.UNKN3 = binReader.ReadUInt16();
            MissionBuilder.UNKN4 = binReader.ReadUInt16();
            MissionBuilder.TIME = binReader.ReadUInt16();
            MissionBuilder.UNKN6 = binReader.ReadUInt16();
            MissionBuilder.UNKN7 = binReader.ReadUInt16();
            MissionBuilder.UNKN8 = binReader.ReadUInt16();
            MissionBuilder.EXC1 = binReader.ReadUInt16();
            MissionBuilder.EXC2 = binReader.ReadUInt16();
            MissionBuilder.EXC3 = binReader.ReadUInt16();
            MissionBuilder.EXC4 = binReader.ReadUInt16();
            MissionBuilder.M1TY = binReader.ReadUInt16();
            MissionBuilder.M1HE = binReader.ReadUInt16();
            MissionBuilder.M1SP = binReader.ReadUInt16();
            MissionBuilder.UNKN16 = binReader.ReadUInt16();
            MissionBuilder.UNKN17 = binReader.ReadUInt16();
            MissionBuilder.M2TY = binReader.ReadUInt16();
            MissionBuilder.M2HE = binReader.ReadUInt16();
            MissionBuilder.M2SP = binReader.ReadUInt16();
            MissionBuilder.UNKN21 = binReader.ReadUInt16();
            MissionBuilder.UNKN22 = binReader.ReadUInt16();
        }

        private void ReadStringBlock(string name, BigEndianBinaryReader binReader)
        {
            UInt32 size = binReader.ReadUInt32();
            var value = ReadString(binReader, size);
            MapBuilder.AddBlock(new MapStringDataBlock(name, value));
        }

        private void ReadUInt32Block(string name, BigEndianBinaryReader binReader)
        {
            UInt32 size = binReader.ReadUInt32();

            if (size != 4)
                throw new Exception("Incorrect size for UInt32");

            var value = binReader.ReadUInt32();
            MapBuilder.AddBlock(new MapUInt32DataBlock(name, value));
        }

        #endregion Private Methods

    }
}
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Readers;
using System;
using System.IO;

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
        public readonly MAPFormat Format;

        #region Public Constructors

        public MAPReader(MapBuilder builder, MAPFormat format)
        {
            MapBuilder = builder;
            Format = format;
            PropertiesBuilder = MapPropertiesBuilder.NewPropertiesModel();
            MissionBuilder = MapMissionBuilder.NewMissionModel();
            LayoutBuilder = MapLayoutBuilder.NewLayoutModel();

            PaletteReader = new MAPPaletteReader(this);
        }

        #endregion Public Constructors

        #region Internal Properties

        internal MapLayoutBuilder LayoutBuilder { get; private set; }
        internal MapBuilder MapBuilder { get; private set; }
        internal MapMissionBuilder MissionBuilder { get; private set; }
        internal MAPPaletteReader PaletteReader { get; private set; }
        internal MapPropertiesBuilder PropertiesBuilder { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void Dispose()
        {
        }

        public MapModel Read(Stream stream)
        {
            //We dont want to close the stream here so reader is not being used inside of 'using' statement
            BigEndianBinaryReader binReader = new BigEndianBinaryReader(stream);

            PropertiesBuilder.Header = ReadHeader(binReader);

            while (!IsEOF(binReader))
            {
                string string_result = ReadString(binReader, 4);

                if (string_result == "XBLK")
                    PropertiesBuilder.XBLK = (int)ReadUInt32Block(binReader);
                else if (string_result == "YBLK")
                    PropertiesBuilder.YBLK = (int)ReadUInt32Block(binReader);
                else if (string_result == "XOFC")
                    PropertiesBuilder.XOFC = (int)ReadUInt32Block(binReader);
                else if (string_result == "YOFC")
                    PropertiesBuilder.YOFC = (int)ReadUInt32Block(binReader);
                else if (string_result == "XOFM")
                    PropertiesBuilder.XOFM = (int)ReadUInt32Block(binReader);
                else if (string_result == "YOFM")
                    PropertiesBuilder.YOFM = (int)ReadUInt32Block(binReader);
                else if (string_result == "XOFA")
                    PropertiesBuilder.XOFA = (int)ReadUInt32Block(binReader);
                else if (string_result == "YOFA")
                    PropertiesBuilder.YOFA = (int)ReadUInt32Block(binReader);
                else if (string_result == "IFFP")
                    PropertiesBuilder.IFFP = ReadStringBlock(binReader);
                else if (string_result == "ALTM")
                    PropertiesBuilder.ALTM = ReadStringBlock(binReader);
                else if (string_result == "ALTP")
                    PropertiesBuilder.ALTP = ReadStringBlock(binReader);
                else if (string_result == "CMAP")
                    PropertiesBuilder.AddPalette(ReadColorBlock(binReader, "CMAP"));
                else if (string_result == "ALCM")
                    PropertiesBuilder.AddPalette(ReadColorBlock(binReader, "ALCM"));
                else if (string_result == "IFFC")
                    PropertiesBuilder.AddPalette(ReadColorBlockEx(binReader, "IFFC"));
                else if (string_result == "CCCI")
                    PropertiesBuilder.CCCI = ReadBytesBlock(binReader);
                else if (string_result == "CCIN")
                    PropertiesBuilder.CCIN = ReadBytesBlock(binReader);
                else if (string_result == "CSIN")
                    PropertiesBuilder.CSIN = ReadBytesBlock(binReader);
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
                    ReadBody(binReader);
                else
                    ReadStringBlock(binReader);
                //throw new Exception(string_result + ": Unknown block!");
            }

            MapBuilder.SetProperties(PropertiesBuilder.Build());
            MapBuilder.SetMission(MissionBuilder.Build());
            MapBuilder.SetBody(LayoutBuilder.Build());

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

        private void ReadBody(BigEndianBinaryReader binReader)
        {
            int sizeX = PropertiesBuilder.XBLK;
            int sizeY = PropertiesBuilder.YBLK;

            //Check how many tile can be read from file and how many are expected based on map sizes
            UInt32 tilesNo = binReader.ReadUInt32() / 2;
            int expectedTilesNo = sizeX * sizeY;

            if (tilesNo != expectedTilesNo)
                throw new Exception("Incorrect number of tiles in body (" + tilesNo + "). Expected: " + expectedTilesNo);

            var gfxLayerBuilder = MapLayoutLayerBuilder<TileRef>.NewMapLayoutLayerModel();
            var propertyLayerBuilder = MapLayoutLayerBuilder<int>.NewMapLayoutLayerModel();

            gfxLayerBuilder.SetName("GFX");
            gfxLayerBuilder.SetSize(sizeX, sizeY);
            propertyLayerBuilder.SetName("PROP");
            propertyLayerBuilder.SetSize(sizeX, sizeY);

            LayoutBuilder.SetSize(sizeX, sizeY);

            for (int i = 0; i < tilesNo; i++)
            {
                UInt16 data = binReader.ReadUInt16();
                int gfxId;
                int propId;

                if (Format == MAPFormat.ABSE)
                {
                    gfxId = (UInt16)(data << 7) >> 7;
                    propId = data >> 9;
                }
                else
                {
                    propId = (UInt16)(data << 10) >> 10;
                    gfxId = data >> 6;
                }

                gfxLayerBuilder.SetCell(i, new TileRef(0, gfxId));
                propertyLayerBuilder.SetCell(i, propId);
            }

            LayoutBuilder.AddLayer(gfxLayerBuilder.Build());
            LayoutBuilder.AddLayer(propertyLayerBuilder.Build());
        }

        private byte[] ReadBytesBlock(BigEndianBinaryReader binReader)
        {
            UInt32 size = binReader.ReadUInt32();
            return binReader.ReadBytes((int)size);
        }

        private PaletteModel ReadColorBlock(BigEndianBinaryReader binReader, string name)
        {
            PaletteModel result = PaletteReader.Read(binReader);
            result.Name = name;
            return result;
        }

        private PaletteModel ReadColorBlockEx(BigEndianBinaryReader binReader, string name)
        {
            PaletteModel result = PaletteReader.ReadEx(binReader);
            result.Name = name;
            return result;
        }

        private byte[] ReadHeader(BigEndianBinaryReader binReader)
        {
            //Read file header. This should contain editor name
            return binReader.ReadBytes(12);
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

        private string ReadStringBlock(BigEndianBinaryReader binReader)
        {
            UInt32 size = binReader.ReadUInt32();
            return ReadString(binReader, size);
        }

        private UInt32 ReadUInt32Block(BigEndianBinaryReader binReader)
        {
            UInt32 size = binReader.ReadUInt32();

            if (size != 4)
                throw new Exception("Incorrect size for UInt32");

            return binReader.ReadUInt32();
        }

        #endregion Private Methods

    }
}
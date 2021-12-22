using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Common.Readers;
using System;
using System.IO;
using System.Linq;
using OpenBreed.Reader.Maps;
using OpenBreed.Common.Tools;

namespace OpenBreed.Reader.Legacy.Maps.MAP
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
        }

        #endregion Public Constructors

        #region Internal Properties

        internal MapBuilder MapBuilder { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void Dispose()
        {
        }

        public MapModel Read(Stream stream)
        {
            //We dont want to close the stream here so reader is not being used inside of 'using' statement
            var binReader = new BigEndianBinaryReader(stream);

            ReadHeader(binReader);

            while (!IsEOF(binReader))
                ReadBlock(binReader);

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

        private void ReadBlock(BigEndianBinaryReader binReader)
        {
            string blockName = ReadString(binReader, 4);

            switch (blockName)
            {
                case "XBLK":
                    ReadUInt32Block("XBLK", binReader);
                    break;
                case "YBLK":
                    ReadUInt32Block("YBLK", binReader);
                    break;
                case "XOFC":
                    ReadUInt32Block("XOFC", binReader);
                    break;
                case "YOFC":
                    ReadUInt32Block("YOFC", binReader);
                    break;
                case "XOFM":
                    ReadUInt32Block("XOFM", binReader);
                    break;
                case "YOFM":
                    ReadUInt32Block("YOFM", binReader);
                    break;
                case "XOFA":
                    ReadUInt32Block("XOFA", binReader);
                    break;
                case "YOFA":
                    ReadUInt32Block("YOFA", binReader);
                    break;
                case "IFFP":
                    ReadStringBlock("IFFP", binReader);
                    break;
                case "ALTM":
                    ReadStringBlock("ALTM", binReader);
                    break;
                case "ALTP":
                    ReadStringBlock("ALTP", binReader);
                    break;
                case "CMAP":
                    ReadPaletteBlock("CMAP", binReader);
                    break;
                case "ALCM":
                    ReadPaletteBlock("ALCM", binReader);
                    break;
                case "IFFC":
                    ReadBytesBlock("IFFC", binReader);
                    break;
                case "CCCI":
                    ReadBytesBlock("CCCI", binReader);
                    break;
                case "CCIN":
                    ReadBytesBlock("CCIN", binReader);
                    break;
                case "CSIN":
                    ReadBytesBlock("CSIN", binReader);
                    break;
                case "MISS":
                    ReadMissionBlock("MISS", binReader);
                    break;
                case "MTXT":
                    ReadTextBlock("MTXT", binReader);
                    break;
                case "LCTX":
                    ReadTextBlock("LCTX", binReader);
                    break;
                case "NOT1":
                    ReadTextBlock("NOT1", binReader);
                    break;
                case "NOT2":
                    ReadTextBlock("NOT2", binReader);
                    break;
                case "NOT3":
                    ReadTextBlock("NOT3", binReader);
                    break;
                case "BODY":
                    ReadBodyBlock(binReader);
                    break;
                default:
                    ReadBytesBlock("UKWN", binReader);
                     break;
            }
        }

        private void ReadBodyBlock(BigEndianBinaryReader binReader)
        {
            int sizeX = (int)MapBuilder.Blocks.OfType<MapUInt32Block>().FirstOrDefault(item => item.Name == "XBLK").Value;
            int sizeY = (int)MapBuilder.Blocks.OfType<MapUInt32Block>().FirstOrDefault(item => item.Name == "YBLK").Value;

            //Check how many tile can be read from file and how many are expected based on map sizes
            var tilesNo = (int)(binReader.ReadUInt32() / 2);
            int expectedTilesNo = sizeX * sizeY;

            if (tilesNo != expectedTilesNo)
                throw new InvalidDataException("Incorrect number of tiles in body (" + tilesNo + "). Expected: " + expectedTilesNo);

            var layout = MapBuilder.CreateLayout();
            layout.SetCellSize(16);
            layout.SetSize(sizeX, sizeY);
            var gfxLayerBuilder = layout.AddLayer(MapLayerType.Gfx);
            gfxLayerBuilder.SetVisible(true);
            var actionLayerBuilder = layout.AddLayer(MapLayerType.Action);
            actionLayerBuilder.SetVisible(true);

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

                var x = i % sizeX;
                var y = i / sizeX;

                gfxLayerBuilder.SetValue(x, y, gfxId);
                actionLayerBuilder.SetValue(x, y, actionId);
            }
        }

        private void ReadBytesBlock(string name, BigEndianBinaryReader binReader)
        {
            UInt32 size = binReader.ReadUInt32();
            var value = binReader.ReadBytes((int)size);
            MapBuilder.AddBlock(new MapUnknownBlock(name, value));
        }

        private void ReadHeader(BigEndianBinaryReader binReader)
        {
            //Read file header. This should contain editor name
            var header = binReader.ReadBytes(12);
            MapBuilder.SetHeader(header);
        }

        private void ReadMissionBlock(string name, BigEndianBinaryReader binReader)
        {
            var missionBlock = new MapMissionBlock(name);

            UInt32 uint_size = binReader.ReadUInt32();

            //NOTE: For some reason there is 4 bytes reading offset when MISS block is used in ABTA map files.
            uint_size -= 4;

            missionBlock.UNKN1 = binReader.ReadUInt16();
            missionBlock.UNKN2 = binReader.ReadUInt16();
            missionBlock.UNKN3 = binReader.ReadUInt16();
            missionBlock.UNKN4 = binReader.ReadUInt16();
            missionBlock.TIME = binReader.ReadUInt16();
            missionBlock.UNKN6 = binReader.ReadUInt16();
            missionBlock.UNKN7 = binReader.ReadUInt16();
            missionBlock.UNKN8 = binReader.ReadUInt16();
            missionBlock.EXC1 = binReader.ReadUInt16();
            missionBlock.EXC2 = binReader.ReadUInt16();
            missionBlock.EXC3 = binReader.ReadUInt16();
            missionBlock.EXC4 = binReader.ReadUInt16();
            missionBlock.M1TY = binReader.ReadUInt16();
            missionBlock.M1HE = binReader.ReadUInt16();
            missionBlock.M1SP = binReader.ReadUInt16();
            missionBlock.UNKN16 = binReader.ReadUInt16();
            missionBlock.UNKN17 = binReader.ReadUInt16();
            missionBlock.M2TY = binReader.ReadUInt16();
            missionBlock.M2HE = binReader.ReadUInt16();
            missionBlock.M2SP = binReader.ReadUInt16();
            missionBlock.UNKN21 = binReader.ReadUInt16();
            missionBlock.UNKN22 = binReader.ReadUInt16();

            MapBuilder.AddBlock(missionBlock);
        }

        private void ReadPaletteBlock(string name, BigEndianBinaryReader binReader)
        {
            var uIntSize = binReader.ReadUInt32();
            int colorsNo = (Int32)uIntSize / 3;
            var value = new MapPaletteBlock.ColorData[colorsNo];

            for (int i = 0; i < colorsNo; i++)
            {
                var colorBytes = binReader.ReadBytes(3);
                value[i] = new MapPaletteBlock.ColorData(colorBytes[0], colorBytes[1], colorBytes[2]);
            }

            MapBuilder.AddBlock(new MapPaletteBlock(name, value));
        }

        private void ReadStringBlock(string name, BigEndianBinaryReader binReader)
        {
            UInt32 size = binReader.ReadUInt32();
            var value = ReadString(binReader, size);
            MapBuilder.AddBlock(new MapStringBlock(name, value));
        }

        private void ReadTextBlock(string name, BigEndianBinaryReader binReader)
        {
            var size = binReader.ReadUInt32();
            //NOTE: For some reason there is 4 bytes reading offset when MISS block or text blocks are used in ABTA map files.
            size -= 4;
            var text = ReadString(binReader, size);
            MapBuilder.AddBlock(new MapTextBlock(name, text));
        }

        private void ReadUInt32Block(string name, BigEndianBinaryReader binReader)
        {
            UInt32 size = binReader.ReadUInt32();

            if (size != 4)
                throw new InvalidDataException("Incorrect size for UInt32");

            var value = binReader.ReadUInt32();
            MapBuilder.AddBlock(new MapUInt32Block(name, value));
        }

        #endregion Private Methods

    }
}
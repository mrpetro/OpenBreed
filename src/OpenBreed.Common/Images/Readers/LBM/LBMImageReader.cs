using OpenBreed.Common.Images.Builders;
using OpenBreed.Common.Images.Readers.LBM.Helpers;
using OpenBreed.Common.Readers;
using OpenBreed.Common.Tiles.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenBreed.Common.Images.Readers.LBM.Helpers.BMHDBlock;

namespace OpenBreed.Common.Images.Readers.LBM
{
    public class LBMImageReader
    {

        #region Public Constructors

        public LBMImageReader(ImageBuilder builder)
        {
            Builder = builder;
        }

        #endregion Public Constructors

        #region Internal Properties

        internal ImageBuilder Builder { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        private string formatId;
        private BMHDBlock bmhdBlock;
        private Color[] cmapBlock;

        private void ReadCMAPBlock(BigEndianBinaryReader binReader, int blockLength)
        {
            int colorsNo = (Int32)blockLength / 3;
            cmapBlock = new Color[colorsNo];

            for (int i = 0; i < colorsNo; i++)
            {
                var colorBytes = binReader.ReadBytes(3);
                cmapBlock[i] = Color.FromArgb(colorBytes[0],
                                              colorBytes[1], 
                                              colorBytes[2]);
            }
        }

        private void ReadBMHDBlock(BigEndianBinaryReader binReader, int blockLength)
        {
            bmhdBlock = new BMHDBlock();
            bmhdBlock.Width = binReader.ReadUInt16();
            bmhdBlock.Height = binReader.ReadUInt16();
            bmhdBlock.Xorigin = binReader.ReadInt16();
            bmhdBlock.Yorigin = binReader.ReadInt16();
            bmhdBlock.NumPlanes = binReader.ReadByte();
            bmhdBlock.Mask = (MaskEnum)binReader.ReadByte();
            bmhdBlock.Compression = (CompressionEnum)binReader.ReadByte();
            bmhdBlock.Pad1 = binReader.ReadByte();
            bmhdBlock.TransClr = binReader.ReadUInt16();
            bmhdBlock.Xaspect = binReader.ReadByte();
            bmhdBlock.Yaspect = binReader.ReadByte();
            bmhdBlock.PageWidth = binReader.ReadInt16();
            bmhdBlock.PageHeight = binReader.ReadInt16();
        }

        internal string ReadString(BigEndianBinaryReader binReader, int size)
        {
            var data = binReader.ReadBytes(size);
            return Encoding.ASCII.GetString(data);
        }

        private void ReadBlock(BigEndianBinaryReader binReader)
        {
            string blockName = ReadString(binReader, 4);
            int blockLength = binReader.ReadInt32();

            switch (blockName)
            {
                case "BMHD":
                    ReadBMHDBlock(binReader, blockLength);
                    break;
                case "CMAP":
                    ReadCMAPBlock(binReader, blockLength);
                    break;
                case "BODY":
                    ReadBODYBlock(binReader, blockLength);
                    break;
                default:
                    IgnoreUnknownBlock(binReader, blockLength);
                    break;
            }

            //Read optional padding byte, only present if lenChunk is not a multiple of 2.
            if (blockLength % 2 != 0)
                binReader.ReadByte();
        }

        private void ProcessILBM(byte[] bytes)
        {
            int bytesNo = bmhdBlock.Width * bmhdBlock.Height / 8;

            byte[] imageData = new byte[bmhdBlock.Width * bmhdBlock.Height];

            for (int j = 0; j < bmhdBlock.Height; j++)
            {
                var linePixels = new byte[bmhdBlock.Width];
                //scan planes
                for (int i = 0; i < bmhdBlock.NumPlanes; i++)
                {
                    byte[] rawData = bytes.Take(bmhdBlock.Width / 8).ToArray();
                    rawData = rawData.Reverse().ToArray();

                    var bitArray = new BitArray(rawData);
                    for (int iPixel = 0; iPixel < bmhdBlock.Width; iPixel++)
                    {
                        var bit = bitArray[iPixel];
                        linePixels[iPixel] = (byte)(linePixels[iPixel] | ((byte)((bit ? 1 : 0) << (i))));
                    }

                    bytes = bytes.Skip(bmhdBlock.Width / 8).ToArray();
                }

                linePixels.CopyTo(imageData, (bmhdBlock.Height - j - 1) * bmhdBlock.Width);
            }

            imageData = imageData.Reverse().ToArray();

            Builder.SetSize(bmhdBlock.Width, bmhdBlock.Height);
            Builder.SetPixelFormat(PixelFormat.Format8bppIndexed);
            Builder.SetData(imageData);
            Builder.SetPalette(cmapBlock);
        }

        private byte[] ExpandBytes(byte[] bytes)
        {
            switch (bmhdBlock.Compression)
            {
                case CompressionEnum.Uncompressed:
                    return bytes;
                case CompressionEnum.RLECompressed:
                    return RLEDecompress(bytes);
                default:
                    throw new NotImplementedException($"Compression '{bmhdBlock.Compression}'");
            }
        }

        private void ProcessPBM(byte[] bytes)
        {
            Builder.SetSize(bmhdBlock.Width, bmhdBlock.Height);
            Builder.SetPixelFormat(PixelFormat.Format8bppIndexed);
            Builder.SetData(bytes);
            Builder.SetPalette(cmapBlock);
        }

        private void ReadBODYBlock(BigEndianBinaryReader binReader, int blockLength)
        {
            if (bmhdBlock == null)
                throw new InvalidDataException("'BMHD' block expected first");

            if (cmapBlock == null)
                throw new InvalidDataException("'CMAP' block expected first");

            var bytesPerRow = 2 * ((bmhdBlock.Width + 15) / 16);

            var bytes = binReader.ReadBytes(blockLength);

            //Decompress if needed
            bytes = ExpandBytes(bytes);

            if (formatId == "ILBM")
                ProcessILBM(bytes);
            else if (formatId == "PBM ")
                ProcessPBM(bytes);
            else
                throw new NotImplementedException(formatId);
        }

        private byte[] RLEDecompress(byte[] bytes)
        {
            var result = new List<Byte>();
            for (int i = 0; i < bytes.Length; i++)
            {
                var n = (sbyte)bytes[i];

                //Read and output the next[value + 1] bytes
                if (n >= 0 && n <= 127)
                {
                    for (int j = 0; j < n + 1; j++)
                    {
                        i++;
                        result.Add(bytes[i]);
                    }
                }
                //Read the next byte and output it(257 - [Value]) times.
                else if (n >= -127 && n <= -1)
                {
                    for (int j = 0; j < -n + 1; j++)
                        result.Add(bytes[i + 1]);
                    i++;
                }
                //Exit the loop
                else if (n == -128)
                    break;
            }

            return result.ToArray();
        }

        private void IgnoreUnknownBlock(BigEndianBinaryReader binReader, int blockLength)
        {
            binReader.ReadBytes(blockLength);
        }

        private void ReadHeader(BigEndianBinaryReader binReader)
        {
            var chunkId = ReadString(binReader, 4);

            if (chunkId != "FORM")
                throw new InvalidDataException("Expected 'FORM' header");

            var lenChunk = binReader.ReadUInt32();

            formatId = ReadString(binReader, 4);

            if (formatId != "ILBM" && formatId != "PBM ")
                throw new InvalidDataException("Expected 'ILBM' or 'PBM ' format id");
        }

        public Image Read(Stream stream)
        {
            //We dont want to close the stream here so reader is not being used inside of 'using' statement
            var binReader = new BigEndianBinaryReader(stream);

            ReadHeader(binReader);

            while (!IsEOF(binReader))
                ReadBlock(binReader);

            return Builder.Build();
        }

        internal bool IsEOF(BigEndianBinaryReader binReader)
        {
            return binReader.BaseStream.Position == binReader.BaseStream.Length;
        }

        public static Color From32Bit(UInt32 value)
        {
            byte b = (byte)(((value & 0x000000FF) ));
            byte g = (byte)(((value & 0x0000FFFF) >> 8));
            byte r = (byte)(((value & 0x00FFFFFF) >> 16));
            return Color.FromArgb(255, r, g, b);
        }

        public static Color From16Bit(UInt16 value)
        {
            byte r = (byte)(((value & 0x0FFF) >> 8) * 17);
            byte g = (byte)(((value & 0x00FF) >> 4) * 17);
            byte b = (byte)(((value & 0x000F)) * 17);
            return Color.FromArgb(255, r, g, b);
        }


        #endregion Public Methods

    }
}

﻿using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Readers.Images.IFF.Helpers;
using OpenBreed.Model.Images;
using OpenBreed.Reader.Images;
using OpenBreed.Reader.Legacy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static OpenBreed.Common.Readers.Images.IFF.Helpers.BMHDBlock;

namespace OpenBreed.Common.Readers.Images.IFF
{
    /// <summary>
    /// Interchange File Format (IFF) image data reader class
    /// Implementation source: https://en.wikipedia.org/wiki/Interchange_File_Format
    /// Currently this implementation supports two image formats:
    /// ILBM and PBM - Implementation source: https://en.wikipedia.org/wiki/ILBM
    /// </summary>
    public class LBMImageReader : IImageReader
    {
        #region Private Fields

        private readonly ImageBuilder builder;
        private BMHDBlock bmhdBlock;
        private MyColor[] cmapBlock;
        private string formatId;

        #endregion Private Fields

        #region Public Constructors

        public LBMImageReader(ImageBuilder builder)
        {
            this.builder = builder;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Read IFF data from stream and produce Image object
        /// </summary>
        /// <param name="stream">Stream to data containing IFF data</param>
        /// <returns>Resulting image</returns>
        public IImage Read(Stream stream)
        {
            //We dont want to close the stream here so reader is not being used inside of 'using' statement
            var binReader = new BigEndianBinaryReader(stream);

            ReadHeader(binReader);

            while (!IsEOF(binReader))
                ReadBlock(binReader);

            return builder.Build();
        }

        #endregion Public Methods

        #region Private Methods

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

        private void IgnoreUnknownBlock(BigEndianBinaryReader binReader, int blockLength)
        {
            binReader.ReadBytes(blockLength);
        }

        private bool IsEOF(BigEndianBinaryReader binReader)
        {
            return binReader.BaseStream.Position == binReader.BaseStream.Length;
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
                    byte[] rawData = bytes.Take(bmhdBlock.Width / 8).Reverse().ToArray();

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

            builder.SetSize(bmhdBlock.Width, bmhdBlock.Height);
            builder.SetPixelFormat(MyPixelFormat.Format8bppIndexed);
            builder.SetData(imageData);
            builder.SetPalette(cmapBlock);
        }

        private void ProcessPBM(byte[] bytes)
        {
            builder.SetSize(bmhdBlock.Width, bmhdBlock.Height);
            builder.SetPixelFormat(MyPixelFormat.Format8bppIndexed);
            builder.SetData(bytes);
            builder.SetPalette(cmapBlock);
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

        private void ReadCMAPBlock(BigEndianBinaryReader binReader, int blockLength)
        {
            int colorsNo = (Int32)blockLength / 3;
            cmapBlock = new MyColor[colorsNo];

            for (int i = 0; i < colorsNo; i++)
            {
                var colorBytes = binReader.ReadBytes(3);
                cmapBlock[i] = MyColor.FromArgb(colorBytes[0],
                                              colorBytes[1],
                                              colorBytes[2]);
            }
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

        private string ReadString(BigEndianBinaryReader binReader, int size)
        {
            var data = binReader.ReadBytes(size);
            return Encoding.ASCII.GetString(data);
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

        #endregion Private Methods
    }
}
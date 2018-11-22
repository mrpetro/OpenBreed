using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OpenBreed.Common.Writers
{
    public class BigEndianBinaryWriter : BinaryWriter
    {
        private byte[] a16 = new byte[2];
        private byte[] a32 = new byte[4];
        private byte[] a64 = new byte[8];

        public BigEndianBinaryWriter(Stream stream) :
            base(stream)
        {
        }

        public override void Write(decimal value)
        {
            throw new NotImplementedException();
        }

        public override void Write(double value)
        {
            a64 = BitConverter.GetBytes(value);
            Array.Reverse(a64);
            base.Write(a64);
        }

        public override void Write(Int16 value)
        {
            a16 = BitConverter.GetBytes(value);
            Array.Reverse(a16);
            base.Write(a16);
        }

        public override void Write(Int32 value)
        {
            a32 = BitConverter.GetBytes(value);
            Array.Reverse(a32);
            base.Write(a32);
        }

        public override void Write(Int64 value)
        {
            a64 = BitConverter.GetBytes(value);
            Array.Reverse(a64);
            base.Write(a64);
        }

        public override void Write(float value)
        {
            a32 = BitConverter.GetBytes(value);
            Array.Reverse(a32);
            base.Write(a32);
        }

        public override void Write(UInt16 value)
        {
            a16 = BitConverter.GetBytes(value);
            Array.Reverse(a16);
            base.Write(a16);
        }

        public override void Write(UInt32 value)
        {
            a32 = BitConverter.GetBytes(value);
            Array.Reverse(a32);
            base.Write(a32);
        }

        public override void Write(UInt64 value)
        {
            a64 = BitConverter.GetBytes(value);
            Array.Reverse(a64);
            base.Write(a64);
        }
    }
}

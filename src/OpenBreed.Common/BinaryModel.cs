using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public class BinaryModel
    {
        public BinaryModel(byte[] bytes)
        {
            Bytes = bytes;
            Stream = new MemoryStream(Bytes);
        }

        #region Public Properties

        public byte[] Bytes { get; }

        #endregion Public Properties

        public Stream Stream { get; }
    }
}

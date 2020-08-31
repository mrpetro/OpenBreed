using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Model.Sounds
{
    public class SoundModel
    {

        #region Public Properties

        public int BitsPerSample { get; set; }
        public int Channels { get; set; }
        public byte[] Data { get; set; }
        public int SampleRate { get; set; }

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        #endregion Public Properties
    }
}

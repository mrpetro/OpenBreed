using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Audio.OpenAL.Managers
{
    internal class SoundSource
    {
        #region Public Constructors

        public SoundSource(int id, int alSourceId)
        {
            Id = id;
            ALSourceId = alSourceId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id { get; }
        public SampleStream CurrentStream { get; internal set; }

        #endregion Public Properties

        #region Internal Properties

        internal int ALSourceId { get; }

        #endregion Internal Properties
    }
}

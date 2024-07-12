using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio;

namespace OpenBreed.Audio.OpenAL
{
    internal static class ALTools
    {
        #region Public Methods

        public static ALSourceState GetSourceState(int source)
        {
            AL.GetSource(source, ALGetSourcei.SourceState, out int state);
            return (ALSourceState)state;
        }

        #endregion Public Methods
    }
}
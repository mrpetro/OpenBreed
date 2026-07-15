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

        public static SourceState GetSourceState(int source)
        {
            AL.GetSourcei(source, SourceGetPNameI.SourceState, out int state);
            return (SourceState)state;
        }

        #endregion Public Methods
    }
}
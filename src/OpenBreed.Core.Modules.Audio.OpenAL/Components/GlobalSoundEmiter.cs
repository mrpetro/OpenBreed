using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.Modules.Audio.Components
{
    /// <summary>
    /// This sound emiter implementation is used for emiting sound on entire owning world
    /// Required components:
    ///     None
    /// </summary>
    public class GlobalSoundEmiter : ISoundEmiter
    {
        #region Public Constructors

        public GlobalSoundEmiter()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Play(string soundId)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
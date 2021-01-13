using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Components.Audio
{
    /// <summary>
    /// This sound emiter implementation is used for emiting sound on entire owning world
    /// Required components:
    ///     None
    /// </summary>
    public class GlobalSoundEmiter : IEntityComponent
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
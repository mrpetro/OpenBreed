using OpenBreed.Components.Common;
using System;
using System.Linq;

namespace OpenBreed.Components.Audio
{
    /// <summary>
    /// This sound emiter implementation is used for emiting sound from particular position in owning world coordinates
    /// Required components:
    ///     - Position
    /// </summary>
    public class LocalSoundEmiter : IEntityComponent
    {
        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public LocalSoundEmiter()
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
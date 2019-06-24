using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Common.Components;
using System;
using System.Linq;

namespace OpenBreed.Core.Modules.Audio.Components
{
    /// <summary>
    /// This sound emiter implementation is used for emiting sound from particular position in owning world coordinates
    /// Required components:
    ///     - Position
    /// </summary>
    public class LocalSoundEmiter : ISoundEmiter
    {
        #region Private Fields

        private IPosition position;

        #endregion Private Fields

        #region Public Constructors

        public LocalSoundEmiter()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public Type SystemType { get { return typeof(SoundSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
        }

        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<IPosition>().First();
        }

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
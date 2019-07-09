using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Modules.Audio.Components
{
    /// <summary>
    /// Sound emiter component interface
    /// </summary>
    public interface ISoundEmiter : IEntityComponent
    {
        #region Public Methods

        /// <summary>
        /// Play sound with given id
        /// </summary>
        /// <param name="soundId">Id of sound to play</param>
        void Play(string soundId);

        /// <summary>
        /// Stop currently played sound
        /// </summary>
        void Stop();

        /// <summary>
        /// Pause currently played sound
        /// </summary>
        void Pause();

        /// <summary>
        /// Resume currently paused sound
        /// </summary>
        void Resume();

        #endregion Public Methods
    }
}
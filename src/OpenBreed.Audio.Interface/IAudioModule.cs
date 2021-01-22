using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;

namespace OpenBreed.Audio.Interface
{
    /// <summary>
    /// Core module interface specialized in sounds and music related work
    /// </summary>
    public interface IAudioModule : ICoreModule
    {
        #region Public Properties

        ISoundMan Sounds { get; }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods
    }
}
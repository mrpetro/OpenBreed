using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Common;

namespace OpenBreed.Wecs.Systems.Audio.Extensions
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupAudioSystems(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new SoundSystem(manCollection.GetManager<ISoundMan>()));
        }

        #endregion Public Methods
    }
}
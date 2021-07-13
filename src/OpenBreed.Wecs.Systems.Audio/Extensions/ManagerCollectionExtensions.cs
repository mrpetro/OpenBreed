using OpenBreed.Common;

namespace OpenBreed.Wecs.Systems.Audio.Extensions
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupAudioSystems(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new SoundSystem());
        }

        #endregion Public Methods
    }
}
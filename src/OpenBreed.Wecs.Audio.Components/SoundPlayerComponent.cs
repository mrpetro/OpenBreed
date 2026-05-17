using System.Collections.Generic;

namespace OpenBreed.Wecs.Audio.Components
{
    public interface ISoundPlayerComponentTemplate : IComponentTemplate
    {

    }

    public class SoundPlayerComponent : IEntityComponent
    {
        #region Public Constructors

        public SoundPlayerComponent()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public List<int> ToPlay { get; } = new List<int>();

        #endregion Public Properties
    }
}
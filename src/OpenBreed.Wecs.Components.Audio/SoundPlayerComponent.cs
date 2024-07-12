using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Audio
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

    public sealed class SoundPlayerComponentFactory : ComponentFactoryBase<ISoundPlayerComponentTemplate>
    {
        #region Internal Constructors

        public SoundPlayerComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ISoundPlayerComponentTemplate template)
        {
            return new SoundPlayerComponent();
        }

        #endregion Protected Methods
    }
}
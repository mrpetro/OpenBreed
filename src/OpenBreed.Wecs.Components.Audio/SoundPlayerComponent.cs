namespace OpenBreed.Wecs.Components.Audio
{
    public class SoundPlayerComponent : IEntityComponent
    {
        #region Public Constructors

        public SoundPlayerComponent(int sampleId)
        {
            SampleId = sampleId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int SampleId { get; }

        #endregion Public Properties
    }
}
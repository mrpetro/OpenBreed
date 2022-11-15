namespace OpenBreed.Audio.Interface.Managers
{
    public class NullSoundMan : ISoundMan
    {
        #region Public Constructors

        static NullSoundMan()
        {
            Instance = new NullSoundMan();
        }

        #endregion Public Constructors

        #region Public Properties

        public static NullSoundMan Instance { get; }

        #endregion Public Properties

        #region Public Methods

        public int CreateSoundSource()
        {
            return 0;
        }

        public int CreateStream(string streamName, SoundStreamReader reader)
        {
            return 0;
        }

        public int GetByName(string sampleName)
        {
            return 0;
        }

        public int GetDuration(int sampleId)
        {
            return 0;
        }

        public int LoadSample(string sampleName, byte[] sampleData, int sampleFrequency)
        {
            return 0;
        }

        public int LoadSample(string sampleName, short[] sampleData, int sampleFrequency)
        {
            return 0;
        }

        public void PlaySample(int sampleId)
        {
        }

        public void PlayStream(int sampleStreamId)
        {
        }

        public void Update()
        {
        }

        #endregion Public Methods
    }
}
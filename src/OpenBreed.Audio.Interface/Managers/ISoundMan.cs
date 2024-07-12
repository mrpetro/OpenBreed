namespace OpenBreed.Audio.Interface.Managers
{
    public delegate int SoundStreamReader(int size, short[] buffer);


    /// <summary>
    /// Sounds manager interface
    /// </summary>
    public interface ISoundMan
    {
        #region Public Methods

        /// <summary>
        /// Get sample ID based on it's name
        /// </summary>
        /// <param name="sampleName">Sample name</param>
        /// <returns>ID of given sample name</returns>
        int GetByName(string sampleName);

        /// <summary>
        /// Loads sample from file path with given frequency
        /// </summary>
        /// <param name="sampleName">Name of sample to be assigned to</param>
        /// <param name="sampleData">Sound sample data</param>
        /// <param name="sampleFrequency">Sound sample frequency</param>
        /// <returns>Sound sample ID</returns>
        int LoadSample(string sampleName, byte[] sampleData, int sampleFrequency);

        /// <summary>
        /// Loads sample from file path with given frequency
        /// </summary>
        /// <param name="sampleName">Name of sample to be assigned to</param>
        /// <param name="sampleData">Sound sample data</param>
        /// <param name="sampleFrequency">Sound sample frequency</param>
        /// <returns>Sound sample ID</returns>
        int LoadSample(string sampleName, short[] sampleData, int sampleFrequency);

        int CreateStream(string streamName, SoundStreamReader reader);

        /// <summary>
        /// Play sound sample with particular ID
        /// </summary>
        /// <param name="id">ID of sound sample to play</param>
        void PlaySample(int sampleId);

        /// <summary>
        /// Play sample from stream using particular ID
        /// </summary>
        /// <param name="sampleStreamId">ID of sample stream to play</param>
        void PlayStream(int sampleStreamId);

        /// <summary>
        /// Create sound source
        /// </summary>
        /// <returns>Sound source ID</returns>
        int CreateSoundSource();

        /// <summary>
        /// Get duration (in milliseconds) of sample with given ID
        /// </summary>
        /// <param name="sampleId">ID of sample</param>
        /// <returns>Sample duration in milliseconds</returns>
        int GetDuration(int sampleId);

        void Update();

        #endregion Public Methods
    }
}
namespace OpenBreed.Audio.Interface.Managers
{
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
        /// <param name="sampleFilePath">Sound sample file path</param>
        /// <param name="sampleFrequency">Sound sample frequency</param>
        /// <returns>Sound sample ID</returns>
        int LoadSample(string sampleName, string sampleFilePath, int sampleFrequency);

        /// <summary>
        /// Loads sample from file path with given frequency
        /// </summary>
        /// <param name="sampleName">Name of sample to be assigned to</param>
        /// <param name="sampleData">Sound sample data</param>
        /// <param name="sampleFrequency">Sound sample frequency</param>
        /// <returns>Sound sample ID</returns>
        int LoadSample(string sampleName, byte[] sampleData, int sampleFrequency);

        /// <summary>
        /// Play sound sample with particular ID
        /// </summary>
        /// <param name="id">ID of sound sample to play</param>
        void PlaySample(int id);

        /// <summary>
        /// Create sound source
        /// </summary>
        /// <returns>Sound source ID</returns>
        int CreateSoundSource();

        #endregion Public Methods
    }
}
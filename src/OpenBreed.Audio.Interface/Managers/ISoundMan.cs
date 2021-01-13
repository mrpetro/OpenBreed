using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Audio.Interface.Managers
{
    /// <summary>
    /// Sounds manager interface  which handles creating sounds from various sources
    /// Or just retrieving them by ID or name.
    /// </summary>
    public interface ISoundMan
    {
        /// <summary>
        /// Get sound object by it's ID
        /// </summary>
        /// <param name="id">Given ID of sound</param>
        /// <returns>Return ISound object if found, false otherwise</returns>
        ISound GetById(int id);

        /// <summary>
        /// Creates sound object from audio file path and return it
        /// If id parameter is not set, sound ID will be set to file path
        /// </summary>
        /// <param name="filePath">File path to audio file</param>
        /// <param name="soundId">Optional name of sound to create</param>
        /// <returns>ISound object</returns>
        ISound Load(string filePath, string id = null);

        /// <summary>
        /// Unloads all sounds
        /// </summary>
        void UnloadAll();

        /// <summary>
        /// Play sound with particular id
        /// </summary>
        /// <param name="id">ID number of sound to play</param>
        void PlaySound(int id);
    }
}

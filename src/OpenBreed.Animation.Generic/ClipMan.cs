using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Interface.Logging;
using System;
using System.Collections.Generic;

namespace OpenBreed.Animation.Generic
{
    /// <summary>
    /// Default animation clip manager implementation
    /// </summary>
    /// <typeparam name="TObject">Type of object which is animated</typeparam>
    public class ClipMan<TObject> : IClipMan<TObject>
    {
        private readonly ILogger logger;
        #region Protected Fields

        protected readonly IClip<TObject> missingClip;

        #endregion Protected Fields

        #region Private Fields

        private readonly List<IClip<TObject>> clips = new List<IClip<TObject>>();
        private readonly Dictionary<string, IClip<TObject>> names = new Dictionary<string, IClip<TObject>>();

        #endregion Private Fields

        #region Public Constructors

        public ClipMan(ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger);

            this.logger = logger;
            this.missingClip = CreateClip("Animations/Missing", 1.0f);
        }

        #endregion Public Constructors

        #region Public Methods

        public IClip<TObject> CreateClip(string name, float length)
        {
            var newClip = new Clip<TObject>(clips.Count, name, length);
            clips.Add(newClip);
            names.Add(name, newClip);
            return newClip;
        }

        public IClip<TObject> GetById(int id)
        {
            return clips[id];
        }

        /// <summary>
        /// Get animation clip by it's name or missingClip instance if given name was not found
        /// This method will also log missing animation clip occurrence.
        /// </summary>
        /// <param name="name">Name of clip to return</param>
        /// <returns>Animation clip</returns>
        public IClip<TObject> GetByName(string name)
        {
            if (TryGetByName(name, out IClip<TObject> clip))
                return clip;

            logger.LogError("Clip with name '{0}' doesn't exist.", name);
            return missingClip;
        }

        public bool TryGetByName(string name, out IClip<TObject> clip)
        {
            return names.TryGetValue(name, out clip);
        }

        #endregion Public Methods
    }
}
using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic.Builders;
using OpenBreed.Animation.Abstractions;
using OpenBreed.Animation.Abstractions.Builders;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Tools.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

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

        protected readonly IReadOnlyClip<TObject> missingClip;

        #endregion Protected Fields

        #region Private Fields

        private readonly IdMap<IReadOnlyClip<TObject>> clips = new IdMap<IReadOnlyClip<TObject>>();
        private readonly Dictionary<string, int> namesToIdsLookup = new Dictionary<string, int>();

        #endregion Private Fields

        #region Public Constructors

        public ClipMan(ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger);

            this.logger = logger;

            var clipBuilder = NewClip("Animations/Missing", 1.0f);
            missingClip = clipBuilder.Build();
            Register(missingClip);
        }

        #endregion Public Constructors

        #region Public Methods

        public IReadOnlyClip<TObject> GetById(int id)
        {
            return clips[id];
        }

        /// <summary>
        /// Get animation clip by it's name or missingClip instance if given name was not found
        /// This method will also log missing animation clip occurrence.
        /// </summary>
        /// <param name="name">Name of clip to return</param>
        /// <returns>Animation clip</returns>
        public IReadOnlyClip<TObject> GetByName(string name)
        {
            if (TryGetByName(name, out IReadOnlyClip<TObject> clip))
                return clip;

            logger.LogError("Clip with name '{0}' doesn't exist.", name);
            return missingClip;
        }

        /// <summary>
        /// Get animation clip ID by it's name
        /// </summary>
        /// <param name="name">Name of clip to find</param>
        /// <returns>Clip ID</returns>
        /// Throws when not found
        public int GetId(string clipName)
        {
            if (!TryGetId(clipName, out int foundId))
            {
                throw new InvalidOperationException($"Clip with name '{clipName}' is not found.");
            }

            return foundId;
        }

        public bool TryGetId(string name, out int clipId)
        {
            return namesToIdsLookup.TryGetValue(name, out clipId);
        }

        public bool TryGetByName(string name, out IReadOnlyClip<TObject> clip)
        {
            if (!TryGetId(name, out int id))
            {
                clip = null;
                return false;
            }

            clip = clips[id];
            return true;
        }

        public IReadOnlyClipBuilder<TObject> NewClip(string name, float length)
        {
            var clipBuilder = new ReadOnlyClipBuilder<TObject>();
            clipBuilder.SetName(name);
            clipBuilder.SetLength(length);

            return clipBuilder;
        }

        public bool Register(IReadOnlyClip<TObject> clip)
        {
            var newId = clips.NewId();

            if (!namesToIdsLookup.TryAdd(clip.Name, newId))
            {
                return false;
            }

            clips.Add(clip);
            return true;
        }

        #endregion Public Methods
    }
}
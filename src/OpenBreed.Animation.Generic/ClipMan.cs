using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic.Builders;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Builders;
using OpenBreed.Common.Interface.Logging;
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

        private readonly List<IReadOnlyClip<TObject>> clips = new List<IReadOnlyClip<TObject>>();
        private readonly Dictionary<string, IReadOnlyClip<TObject>> names = new Dictionary<string, IReadOnlyClip<TObject>>();

        #endregion Private Fields

        #region Public Constructors

        public ClipMan(ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger);

            this.logger = logger;

            var clipBuilder = NewClip("Animations/Missing", 1.0f);
            this.missingClip = clipBuilder.Build();
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

        public bool TryGetByName(string name, out IReadOnlyClip<TObject> clip)
        {
            return names.TryGetValue(name, out clip);
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
            if (!names.TryAdd(clip.Name, clip))
            {
                return false;
            }

            clip.Id = clips.Count;
            clips.Add(clip);
            return true;
        }

        #endregion Public Methods
    }
}
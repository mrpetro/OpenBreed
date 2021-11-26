using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Animation.Generic
{
    public class ClipMan : IClipMan
    {
        #region Internal Fields

        #endregion Internal Fields

        #region Private Fields

        private readonly List<IClip> clips = new List<IClip>();
        private readonly Dictionary<string, IClip> names = new Dictionary<string, IClip>(); 
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        private IClip MissingAnim;

        public ClipMan(ILogger logger)
        {
            this.logger = logger;

            MissingAnim = CreateClip("Animations/Missing", 1.0f);
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        public IClip CreateClip(string name, float length)
        {
            var newClip = new Clip(clips.Count, name, length);
            clips.Add(newClip);
            names.Add(name, newClip);
            return newClip;
        }

        public IClip GetById(int id)
        {
            return clips[id];
        }

        public IClip GetByName(string name)
        {
            names.TryGetValue(name, out IClip result);
            return result;
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
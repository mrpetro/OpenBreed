using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Animation.Generic
{
    public class ClipMan<TObject> : IClipMan<TObject>
    {
        #region Internal Fields

        #endregion Internal Fields

        #region Private Fields

        private readonly List<IClip<TObject>> clips = new List<IClip<TObject>>();
        private readonly Dictionary<string, IClip<TObject>> names = new Dictionary<string, IClip<TObject>>(); 
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        private IClip<TObject> MissingAnim;

        public ClipMan(ILogger logger)
        {
            this.logger = logger;

            MissingAnim = CreateClip("Animations/Missing", 1.0f);
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

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

        public IClip<TObject> GetByName(string name)
        {
            names.TryGetValue(name, out IClip<TObject> result);
            return result;
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
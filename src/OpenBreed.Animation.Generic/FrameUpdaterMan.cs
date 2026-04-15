using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Abstractions;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Animation.Generic
{
    public class FrameUpdaterMan<TObject> : IFrameUpdaterMan<TObject>
    {
        #region Internal Fields

        #endregion Internal Fields

        #region Private Fields

        private readonly List<(Delegate,Delegate)> pairs = new List<(Delegate, Delegate)>();
        private readonly Dictionary<string, int> namesToIds = new Dictionary<string, int>();
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public FrameUpdaterMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Properties

        public IReadOnlyCollection<string> AnimatorNames => namesToIds.Keys;

        #endregion Public Properties

        #region Public Methods

        public int Register<TValue>(string name, FrameUpdater<TObject, TValue> frameUpdater, FrameLoader<TValue> frameLoader = null)
        {
            pairs.Add((frameUpdater, frameLoader));

            var id = pairs.Count - 1;
            namesToIds.Add(name, id);

            return id;
        }

        public FrameUpdater<TObject, TValue> GetById<TValue>(int id)
        {
            return (FrameUpdater<TObject, TValue>)pairs[id].Item1;
        }

        public FrameLoader<TValue> GetLoaderByName<TValue>(string name)
        {
            if (!namesToIds.TryGetValue(name, out int id))
            {
                return null;
            }

            return (FrameLoader<TValue>)pairs[id].Item2;
        }

        public FrameUpdater<TObject, TValue> GetByName<TValue>(string name)
        {
            if (!namesToIds.TryGetValue(name, out int id))
            {
                logger.LogError("Unable to find frame updater with name '{0}'.", name);
                return null;
            }

            return GetById<TValue>(id);
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
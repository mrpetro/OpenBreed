using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Animation.Generic
{
    public class FrameUpdaterMan<TObject> : IFrameUpdaterMan<TObject>
    {
        #region Internal Fields

        #endregion Internal Fields

        #region Private Fields

        private readonly List<Delegate> items = new List<Delegate>();
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

        #endregion Public Properties

        #region Public Methods

        public int Register<TValue>(string name, FrameUpdater<TObject, TValue> frameUpdater)
        {
            items.Add(frameUpdater);
            var id = items.Count - 1;
            namesToIds.Add(name, id);

            return id;
        }

        public FrameUpdater<TObject, TValue> GetById<TValue>(int id)
        {
            return (FrameUpdater<TObject, TValue>)items[id];
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
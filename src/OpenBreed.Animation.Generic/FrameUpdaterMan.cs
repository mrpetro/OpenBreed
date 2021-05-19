using OpenBreed.Animation.Generic.Helpers;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Animation.Generic
{
    public class FrameUpdaterMan : IFrameUpdaterMan
    {
        #region Internal Fields

        #endregion Internal Fields

        #region Private Fields

        private readonly List<Delegate> items = new List<Delegate>();
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

        public int Register(Delegate frameUpdater)
        {
            items.Add(frameUpdater);
            return items.Count - 1;
        }

        public Delegate GetById(int id)
        {
            return items[id];
        }

        public Delegate GetByName(string name)
        {
            //var anim = items.FirstOrDefault(item => item.Name == name);

            //if (anim != null)
            //    return anim;

            //logger.Error($"Unable to find animation with name '{name}'");

            return null;
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
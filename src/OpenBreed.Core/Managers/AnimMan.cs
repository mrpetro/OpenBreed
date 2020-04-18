using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Managers
{
    public class AnimMan
    {
        #region Internal Fields

        #endregion Internal Fields

        #region Private Fields

        private readonly List<IAnimation> items = new List<IAnimation>();

        #endregion Private Fields

        #region Public Constructors

        public AnimMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public IAnimation<T> Create<T>(string name, Action<IEntity, T> frameUpdateAction)
        {
            var newAnimationData = new Animation<T>(items.Count, name, frameUpdateAction);
            items.Add(newAnimationData);
            return newAnimationData;
        }

        public IAnimation GetById(int id)
        {
            return items[id];
        }

        public IAnimation GetByName(string name)
        {
            return items.FirstOrDefault(item => item.Name == name);
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
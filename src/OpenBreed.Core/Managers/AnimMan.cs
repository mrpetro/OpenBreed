using OpenBreed.Common.Logging;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Managers
{
    public class AnimMan : IAnimMan
    {
        #region Internal Fields

        #endregion Internal Fields

        #region Private Fields

        private readonly List<IAnimation> items = new List<IAnimation>();
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        private IAnimation MissingAnim;

        public AnimMan(ILogger logger)
        {
            this.logger = logger;

            MissingAnim = Create("Animations/Missing", 1.0f);
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        public IAnimation Create(string name, float length)
        {
            var newAnimationData = new Animation(items.Count, name, length);
            items.Add(newAnimationData);
            return newAnimationData;
        }

        public IAnimation GetById(int id)
        {
            return items[id];
        }

        public IAnimation GetByName(string name)
        {
            var anim = items.FirstOrDefault(item => item.Name == name);

            if (anim != null)
                return anim;

            logger.Error($"Unable to find animation with name '{name}'");

            return MissingAnim;
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
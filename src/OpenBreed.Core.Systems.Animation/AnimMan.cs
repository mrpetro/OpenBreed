using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Systems.Animation.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems.Animation
{
    public class AnimMan : IAnimMan
    {
        #region Private Fields

        private readonly List<IAnimationData> items = new List<IAnimationData>();

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

        public IAnimationData<T> Create<T>(string name)
        {
            var newAnimationData = new AnimationData<T>(items.Count, name);
            items.Add(newAnimationData);
            return newAnimationData;
        }

        public IAnimationData GetById(int id)
        {
            return items[id];
        }

        public IAnimationData GetByName(string name)
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
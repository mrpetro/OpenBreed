using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Animation.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Helpers
{
    public class AnimMan : IAnimMan
    {
        #region Private Fields

        internal readonly AnimationModule Module;


        private readonly List<IAnimationData> items = new List<IAnimationData>();

        #endregion Private Fields

        #region Public Constructors

        public AnimMan(AnimationModule module)
        {
            Module = module;
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
using System;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.Entities.Door
{
    internal class AnimationDataFactory
    {
        private readonly Dictionary<string, IAnimationPartLoader> partLoaders = new Dictionary<string, IAnimationPartLoader>();

        public AnimationDataFactory()
        {
        }

        public IAnimationPartLoader GetPartLoader(string animatorType)
        {
            if (partLoaders.TryGetValue(animatorType, out IAnimationPartLoader partLoader))
                return partLoader;
            else
                throw new InvalidOperationException($"Animation Part Loader for animator type '{animatorType}' is not registered");
        }

        internal void Register(string animatorType, IAnimationPartLoader dataLoader)
        {
            partLoaders.Add(animatorType, dataLoader);
        }
    }
}
using System;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.Entities.Door
{
    internal class AnimationDataFactory
    {
        private readonly Dictionary<Type, IAnimationTrackLoader> trackLoaders = new Dictionary<Type, IAnimationTrackLoader>();

        public AnimationDataFactory()
        {
        }

        public IAnimationTrackLoader GetTrackLoader(Type type)
        {
            if (trackLoaders.TryGetValue(type, out IAnimationTrackLoader partLoader))
                return partLoader;
            else
                throw new InvalidOperationException($"Animation Part Loader for animator type '{type}' is not registered");
        }

        internal void Register<TValue>(IAnimationTrackLoader trackLoader)
        {
            trackLoaders.Add(typeof(TValue), trackLoader);
        }
    }
}
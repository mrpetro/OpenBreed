using OpenBreed.Animation.Interface;
using OpenBreed.Ecsw.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Animation.Generic.Helpers
{
    internal class Animation : IAnimation
    {
        #region Private Fields

        private readonly List<IAnimationPart> parts = new List<IAnimationPart>();

        #endregion Private Fields

        #region Internal Constructors

        internal Animation(int id, string name, float length)
        {
            Id = id;
            Name = name;
            Length = length;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int Id { get; }
        public string Name { get; }
        public float Length { get; set; }

        #endregion Public Properties

        #region Public Methods

        public IAnimationPart<T> AddPart<T>(Action<Entity, T> frameUpdateAction, T initialValue)
        {
            var newPart = new AnimationPart<T>(frameUpdateAction, initialValue);
            parts.Add(newPart);
            return newPart;
        }

        public bool UpdateWithNextFrame(Entity entity, IAnimator animator)
        {
            for (int i = 0; i < parts.Count; i++)
                parts[i].UpdateWithNextFrame(entity, animator);

            return true;
        }


        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
        #endregion Public Methods

        #region Private Methods


        #endregion Private Methods
    }
}
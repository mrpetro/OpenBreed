﻿using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using System;

namespace OpenBreed.Core.Modules.Animation.Helpers
{
    /// <summary>
    /// Transition methods between frames
    /// </summary>
    public enum FrameTransition
    {
        /// <summary>
        /// Value of next frame is being set after specific time has passed. It's not smooth.
        /// </summary>
        None,

        /// <summary>
        /// Value of frame is being set based on linear interpolation between current and next frame and passed time.
        /// </summary>
        LinearInterpolation
    }

    public interface IAnimation
    {
        #region Public Properties

        int Id { get; }
        string Name { get; }
        float Length { get; }

        #endregion Public Properties

        #region Public Methods

        bool UpdateWithNextFrame(Entity entity, Animator animator);

        IAnimationPart<T> AddPart<T>(Action<Entity, T> frameUpdateAction, T initialValue);

        #endregion Public Methods
    }

    public interface IAnimationPart
    {
        #region Public Methods

        bool UpdateWithNextFrame(Entity entity, Animator animator);

        #endregion Public Methods
    }

    public interface IAnimationPart<T> : IAnimationPart
    {
        #region Public Methods

        void AddFrame(T value, float frameTime);

        #endregion Public Methods
    }
}
using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Animation
{
    public enum AnimationPlayerActionType
    {
        Play,
        Pause,
        Stop
    }

    public class AnimationPlayerComponent : IEntityComponent
    {
        #region Public Constructors

        public AnimationPlayerComponent(int animatorId, string animationName, AnimationPlayerActionType actionType, float position)
        {
            Set(animatorId, animationName, actionType, position);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<PlayerAction> Actions { get; } = new List<PlayerAction>();

        #endregion Public Properties

        #region Public Methods

        public void Set(int animatorId, string animationName, AnimationPlayerActionType actionType, float position)
        {
            Actions.Add(new PlayerAction() { AnimatorId = animatorId, AnimationName = animationName, ActionType = actionType, Position = position });
        }

        #endregion Public Methods

        #region Public Structs

        public struct PlayerAction
        {
            #region Public Properties

            public int AnimatorId { get; internal set; }
            public string AnimationName { get; internal set; }
            public AnimationPlayerActionType ActionType { get; internal set; }
            public float Position { get; internal set; }

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
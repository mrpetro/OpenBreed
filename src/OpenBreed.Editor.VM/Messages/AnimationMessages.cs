using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Messages
{
    // Create a message
    public abstract class AnimationMessage : CommunityToolkit.Mvvm.Messaging.Messages.RequestMessage<int>
    {
        #region Public Constructors

        public AnimationMessage(int animationId)
        {
        }

        #endregion Public Constructors
    }

    // Create a message
    public class PlayAnimationMessage : AnimationMessage
    {
        #region Public Constructors

        public PlayAnimationMessage(int animationId) : base(animationId)
        {
        }

        #endregion Public Constructors
    }

    // Create a message
    public class StopAnimationMessage : AnimationMessage
    {
        #region Public Constructors

        public StopAnimationMessage(int animationId) : base(animationId)
        {
        }

        #endregion Public Constructors
    }

    // Create a message
    public class PauseAnimationMessage : AnimationMessage
    {
        #region Public Constructors

        public PauseAnimationMessage(int animationId) : base(animationId)
        {
        }

        #endregion Public Constructors
    }

    // Create a message
    public class FastForwardAnimationMessage : AnimationMessage
    {
        #region Public Constructors

        public FastForwardAnimationMessage(int animationId) : base(animationId)
        {
        }

        #endregion Public Constructors
    }

    // Create a message
    public class FastRewindAnimationMessage : AnimationMessage
    {
        #region Public Constructors

        public FastRewindAnimationMessage(int animationId) : base(animationId)
        {
        }

        #endregion Public Constructors
    }

    // Create a message
    public class ToBeginAnimationMessage : AnimationMessage
    {
        #region Public Constructors

        public ToBeginAnimationMessage(int animationId) : base(animationId)
        {
        }

        #endregion Public Constructors
    }

    // Create a message
    public class ToEndAnimationMessage : AnimationMessage
    {
        #region Public Constructors

        public ToEndAnimationMessage(int animationId) : base(animationId)
        {
        }

        #endregion Public Constructors
    }
}
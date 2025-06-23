using OpenBreed.Rendering.Abstractions;

namespace OpenBreed.Editor.UI.Mvc
{
    public interface IAnimationSandboxFactory
    {
        #region Public Methods

        IAnimationSandbox Create();

        #endregion Public Methods
    }

    public interface IAnimationSandbox
    {
        #region Public Methods

        void FastForwardAnimation();

        void FastRewindAnimation();

        void Load(string name);

        void PauseAnimation();

        void PlayAnimation();

        void Render(IRenderView view, float dt);

        void StopAnimation();

        void ToBeginAnimation();

        void ToEndAnimation();

        #endregion Public Methods
    }
}
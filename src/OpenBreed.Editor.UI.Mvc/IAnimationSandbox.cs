using OpenBreed.Common.Interface.Drawing;
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
        #region Public Properties

        float CurrentTime { get; }

        #endregion Public Properties

        #region Public Methods

        MyExtentF GetGraphicalExtent();

        void FastForwardAnimation();

        void FastRewindAnimation();

        void Load(string name, IRenderContext renderContext);

        void PauseAnimation();

        void PlayAnimation();

        void Render(IRenderView view, float dt);

        void StopAnimation();

        void ToBeginAnimation();

        void ToEndAnimation();

        #endregion Public Methods
    }
}
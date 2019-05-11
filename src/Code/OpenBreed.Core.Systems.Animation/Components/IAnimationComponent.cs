using OpenBreed.Core.Common.Components;

namespace OpenBreed.Core.Systems.Animation.Components
{
    public interface IAnimationComponent : IEntityComponent
    {
        #region Public Methods

        void Animate(float dt);

        #endregion Public Methods
    }
}
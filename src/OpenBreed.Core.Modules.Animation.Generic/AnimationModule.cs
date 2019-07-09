using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems;
using System;

namespace OpenBreed.Core.Modules.Animation
{
    public class AnimationModule : IAnimationModule
    {
        #region Private Fields

        private readonly AnimMan animMan;

        #endregion Private Fields

        #region Public Constructors

        public AnimationModule(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));

            animMan = new AnimMan(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public IAnimMan Anims { get { return animMan; } }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create system for handling sprites
        /// </summary>
        /// <returns>Sprite system</returns>
        public IAnimationSystem CreateAnimationSystem<T>()
        {
            return new AnimSystem<T>(Core);
        }

        #endregion Public Methods
    }
}
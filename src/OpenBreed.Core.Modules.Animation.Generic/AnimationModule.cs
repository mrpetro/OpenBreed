using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using System;
using OpenBreed.Core.Managers;

namespace OpenBreed.Core.Modules.Animation
{
    public class AnimationModule : BaseCoreModule, IAnimationModule
    {
        #region Private Fields

        private readonly AnimMan animMan;

        #endregion Private Fields

        #region Public Constructors

        public AnimationModule(ICore core) : base(core)
        {
            animMan = new AnimMan(this);
        }

        #endregion Public Constructors

        #region Public Properties


        public IAnimMan Anims { get { return animMan; } }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods
    }
}
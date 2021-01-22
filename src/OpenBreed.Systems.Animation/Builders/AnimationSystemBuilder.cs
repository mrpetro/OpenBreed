using OpenBreed.Core;
using OpenBreed.Ecsw.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Systems.Animation.Builders
{
    public class AnimationSystemBuilder : IWorldSystemBuilder<AnimationSystem>
    {
        #region Internal Fields

        internal ICore core;

        #endregion Internal Fields

        #region Public Constructors

        public AnimationSystemBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public AnimationSystem Build()
        {
            return new AnimationSystem(this);
        }

        #endregion Public Methods
    }
}

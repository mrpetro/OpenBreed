using OpenBreed.Core.Builders;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Modules.Animation.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Animation.Builders
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

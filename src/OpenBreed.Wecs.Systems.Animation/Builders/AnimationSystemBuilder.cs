using OpenBreed.Animation.Interface;
using OpenBreed.Core;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Animation.Builders
{
    public class AnimationSystemBuilder : ISystemBuilder<AnimationSystem>
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
            return new AnimationSystem(this,
                                       core.GetManager<IEntityMan>(),
                                       core.GetManager<IAnimMan>());
        }

        #endregion Public Methods
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Control.Systems.Extensions
{
    public static class WorldBuilderExtensions
    {
        #region Public Methods

        public static IWorldBuilder AddControlSystems(this IWorldBuilder builder)
        {
            builder.AddSystem<FollowerSystem>();
            builder.AddSystem<AnimatorSystem>();

            return builder;
        }

        #endregion Public Methods
    }
}
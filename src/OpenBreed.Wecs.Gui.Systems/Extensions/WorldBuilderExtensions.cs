using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Gui.Systems.Extensions
{
    public static class WorldBuilderExtensions
    {
        #region Public Methods

        public static IWorldBuilder AddGuiSystems(this IWorldBuilder builder, bool isEditor)
        {
            builder.AddSystem<CollisionVisualizingSystem>();

            if (!isEditor)
            {
                //GUI Stage
                builder.AddSystem<CursorSystem>();
            }

            return builder;
        }

        #endregion Public Methods
    }
}
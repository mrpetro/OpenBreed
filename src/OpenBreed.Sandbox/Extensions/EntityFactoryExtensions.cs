using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class EntityFactoryExtensions
    {
        #region Public Methods

        public static IEntity CreateCamera(this IEntityFactory entityFactory, string name, float x, float y, float width, float height)
        {
            var camera = entityFactory.Create(@"ABTA\Templates\Common\Camera")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetParameter("width", width)
                .SetParameter("height", height)
                .SetTag(name)
            .Build();

            return camera;
        }

        #endregion Public Methods
    }
}
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities
{
    public class EnvironmentHelper
    {
        public EnvironmentHelper(
            IDataLoaderFactory dataLoaderFactory,
            IEntityFactory entityFactory,
            IBuilderFactory builderFactory,
            IWorldMan worldMan)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
            this.builderFactory = builderFactory;
            this.worldMan = worldMan;
        }

        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;
        private readonly IBuilderFactory builderFactory;
        private readonly IWorldMan worldMan;

        public IEntity AddTVFlickering(IWorld world, int x, int y, string level, int gfxValue)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\L4\TVFlickering")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        public IEntity AddMonsterEating(IWorld world, int x, int y, string level, int gfxValue)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\L4\MonsterEating")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

        public IEntity AddShipSmoke(IWorld world, int x, int y, string level, int gfxValue)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\L1\ShipSmoke")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);

            return entity;
        }

    }
}

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
        public EnvironmentHelper(IDataLoaderFactory dataLoaderFactory, IEntityFactory entityFactory, IBuilderFactory builderFactory)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
            this.builderFactory = builderFactory;
        }

        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;
        private readonly IBuilderFactory builderFactory;

        public void AddTVFlickering(World world, int x, int y, string level, int gfxValue)
        {
            var entity = entityFactory.Create(@"Defaults\Templates\ABTA\L4\TVFlickering.xml")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            entity.EnterWorld(world.Id);
        }

        public void AddMonsterEating(World world, int x, int y, string level, int gfxValue)
        {
            var entity = entityFactory.Create(@"Defaults\Templates\ABTA\L4\MonsterEating.xml")
                .SetParameter("level", level)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .Build();

            entity.EnterWorld(world.Id);
        }

    }
}

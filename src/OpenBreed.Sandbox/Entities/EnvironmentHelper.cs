using OpenBreed.Animation.Interface;
using OpenBreed.Common;
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

        public void LoadAnimations()
        {
            var animationLoader = dataLoaderFactory.GetLoader<IClip>();

            animationLoader.Load("Animations/Environment/Level4/TVFlickering");
            animationLoader.Load("Animations/Environment/Level4/MonsterEating");
        }

        public void AddTVFlickering(World world, int x, int y, int atlasId, int gfxValue)
        {
            var entity = entityFactory.Create(@"Entities\Level4\TVFlickering.xml")
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            entity.PutTile(atlasId, gfxValue, 0, new Vector2(16 * x, 16 * y));

            entity.EnterWorld(world.Id);
        }

        public void AddMonsterEating(World world, int x, int y, int atlasId, int gfxValue)
        {
            var entity = entityFactory.Create(@"Entities\Level4\MonsterEating.xml")
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            entity.PutTile(atlasId, gfxValue, 0, new Vector2(16 * x, 16 * y));

            entity.EnterWorld(world.Id);
        }

    }
}

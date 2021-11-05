using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Tools;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
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
        public EnvironmentHelper(IDataLoaderFactory dataLoaderFactory, IEntityFactory entityFactory, ICommandsMan commandMan, IBuilderFactory builderFactory)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
            this.commandMan = commandMan;
            this.builderFactory = builderFactory;
        }

        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;
        private readonly ICommandsMan commandMan;
        private readonly IBuilderFactory builderFactory;

        public void LoadAnimations()
        {
            var animationLoader = dataLoaderFactory.GetLoader<IClip>();

            animationLoader.Load("Animations/Environment/Level4/TVFlickering");
            animationLoader.Load("Animations/Environment/Level4/MonsterEating");
        }

        public void AddTVFlickering(World world, int x, int y, int atlasId, int gfxValue)
        {
            var doorHorizontalTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Level4\TVFlickering.xml");
            var entity = entityFactory.Create(doorHorizontalTemplate);

            entity.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            var tileComponentBuilder = builderFactory.GetBuilder<TileComponentBuilder>();
            tileComponentBuilder.SetAtlasById(atlasId);
            tileComponentBuilder.SetImageIndex(gfxValue);
            entity.Add(tileComponentBuilder.Build());

            entity.EnterWorld(world.Id);
        }

        public void AddMonsterEating(World world, int x, int y, int atlasId, int gfxValue)
        {
            var doorHorizontalTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Level4\MonsterEating.xml");
            var entity = entityFactory.Create(doorHorizontalTemplate);

            entity.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            var tileComponentBuilder = builderFactory.GetBuilder<TileComponentBuilder>();
            tileComponentBuilder.SetAtlasById(atlasId);
            tileComponentBuilder.SetImageIndex(gfxValue);
            entity.Add(tileComponentBuilder.Build());

            entity.EnterWorld(world.Id);
        }

    }
}
